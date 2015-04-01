// ***********************************************************************
// Assembly         : Uninf.Config.Mvc5
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-23-2015
// ***********************************************************************
// <copyright file="ConfigControllerBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config.Mvc5
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// ConfigControllerBase. 类
    /// mvc修改配置界面controller基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigControllerBase<T> : Controller where T : THZConfigBase<T>
    {

        /// <summary>
        /// 显示表单action
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult Index(string section, bool? result)
        {
            this.ViewBag.Section = section;
            this.ViewBag.Result = result;
            var ins = this.GetIns();
            var viewName = this.GetView();
            return View(viewName,ins);
        }
        /// <summary>
        /// 提交表单action
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public virtual ActionResult Index(FormCollection form, string section)
        {
            //ViewBag.Section = section;
            BeforChange(form, section);
            var ins = this.GetIns();
            var sectionProperty = ins.GetType().GetProperty(section);
            if (sectionProperty != null)
            {
                var sectionObj = sectionProperty.GetValue(ins, null);
                if (sectionObj != null)
                {
                    UpdateSection(form,section, sectionProperty, sectionObj);
                    UpdateSuccess(form, section, sectionProperty, sectionObj);
                }

                ins.Save(ins);
                THZConfigHelper<T>.Reload();
                this.SaveSuccess(form,section);
                this.TellOtherServerReloadConfig();
                return this.RedirectToAction(this.GetView(), new { section = section, result = true });
            }
            return this.RedirectToAction(this.GetView(), new { section = section, result = false });
        }

        /// <summary>
        /// 更新section节成功后执行，可override
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        /// <param name="sectionProperty">The section property.</param>
        /// <param name="sectionObj">The section object.</param>
        protected virtual void UpdateSuccess(FormCollection form, string section, PropertyInfo sectionProperty, object sectionObj)
        {
            
        }

        /// <summary>
        /// 更新section节，默认使用反射机制，如有特别需要可override
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        /// <param name="sectionProperty">The section property.</param>
        /// <param name="sectionObj">The section object.</param>
        protected virtual void UpdateSection(FormCollection form,string section,PropertyInfo sectionProperty, object sectionObj)
        {
            try
            {
                var prefix = section;
                Predicate<string> propertyFilter =
                    propertyName => { var p = sectionObj.GetType().GetProperty(propertyName).PropertyType;
                                        if (p.IsArray)
                                        {
                                            var itemtype = p.GetElementType();
                                            if (itemtype == typeof(DateTime)) return true;
                                            if (itemtype == typeof(string)) return true;
                                            return itemtype.IsPrimitive;
                                        }
                                        return true;
                    };
                IModelBinder binder = this.Binders.GetBinder(sectionProperty.PropertyType);

                ModelBindingContext bindingContext = new ModelBindingContext()
                                                         {
                                                             ModelMetadata =
                                                                 ModelMetadataProviders.Current
                                                                 .GetMetadataForType(
                                                                     () => sectionObj,
                                                                     sectionProperty.PropertyType),
                                                             ModelName = prefix,
                                                             ModelState = this.ModelState,
                                                             PropertyFilter = propertyFilter,
                                                             ValueProvider = this.ValueProvider
                                                         };
                binder.BindModel(this.ControllerContext, bindingContext);

                UpdateArrays(form, section,sectionObj);
            }
            catch
            {
                UpdateModel(sectionObj);
            }
        }

        /// <summary>
        /// 更新配置中的数组
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        /// <param name="sectionObj">The section object.</param>
        protected virtual void UpdateArrays(FormCollection form, string section, object sectionObj)
        {
            var arrayProperies = sectionObj.GetType().GetProperties().Where(x => x.PropertyType.IsArray);
            foreach (var p in arrayProperies)
            {
                var x = p.GetValue(sectionObj);
                var arr = x as Array;
                UpdateArray(form, section,p, arr);
            }
        }

        /// <summary>
        /// 更新配置中的数组
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        /// <param name="arrayProperty">The array property.</param>
        /// <param name="array">The array.</param>
        protected virtual void UpdateArray(FormCollection form, string section,PropertyInfo arrayProperty, Array array)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                var thisv = array.GetValue(i);
                var valueType = thisv.GetType();
                foreach (var vp in valueType.GetProperties())
                {
                    UpdateArrayProperty(form, section, arrayProperty, vp, i, thisv);
                }
            }
        }

        /// <summary>
        /// 更新配置中的数组的属性，当数组的元素是复杂类型时
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        /// <param name="arrayProperty">The array property.</param>
        /// <param name="FieldProperty">The field property.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <param name="value">The value.</param>
        protected virtual void UpdateArrayProperty(
            FormCollection form,
            string section,
            PropertyInfo arrayProperty,
            PropertyInfo FieldProperty,int arrayIndex,object value)
        {
            var formName = section + "." + arrayProperty.Name + "[" + arrayIndex + "]." + FieldProperty.Name;
            var v = form[formName];
            if (!string.IsNullOrEmpty(v))
            {
                var vv = this.ValueProvider.GetValue(formName).ConvertTo(FieldProperty.PropertyType);
                FieldProperty.SetValue(value, vv);
            }
        }

        /// <summary>
        /// 保存成功后，通知其他服务器重新载入配置
        /// 当多网站负载时需要实现此方法
        /// </summary>
        protected abstract void TellOtherServerReloadConfig();

        /// <summary>
        /// 保存成功后执行，可override
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        protected virtual void SaveSuccess(FormCollection form, string section)
        {
        }

        /// <summary>
        /// 在开始更新前执行，可override
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="section">The section.</param>
        protected virtual void BeforChange(FormCollection form, string section)
        {
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns>T.</returns>
        protected virtual T GetIns()
        {
            return THZConfigHelper<T>.Instance;
        }

        /// <summary>
        /// 获取view名称
        /// </summary>
        /// <returns>System.String.</returns>
        protected virtual string GetView()
        {
            return "Index";
        }

    }
}
