// ***********************************************************************
// Assembly         : Uninf.Images
// Author           : cz
// Created          : 01-19-2015
//
// Last Modified By : cz
// Last Modified On : 01-19-2015
// ***********************************************************************
// <copyright file="ResizeConfig.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeCarvings.Piczard;

namespace Uninf.Images
{
    /// <summary>
    /// ResizeConfig. 类
    /// </summary>
    public class ResizeConfig
    {
        /// <summary>
        /// 允许小图放大
        /// </summary>
        /// <value><c>true</c> if [resize to bigger]; otherwise, <c>false</c>.</value>
        public bool ResizeToBigger { get; set; }

        /// <summary>
        /// Gets or sets the color of the resize to bigger.
        /// </summary>
        /// <value>The color of the resize to bigger.</value>
        public BackgroundColor ResizeToBiggerColor { get; set; }

        /// <summary>
        /// 要缩放到的宽
        /// </summary>
        /// <value>The width of the resize to.</value>
        public int ResizeToWidth { get; set; }

        /// <summary>
        /// 要缩放到的高
        /// </summary>
        /// <value>The height of the resize to.</value>
        public int ResizeToHeight { get; set; }

        /// <summary>
        /// 允许丢失图片的部分
        /// </summary>
        /// <value><c>true</c> if [allow lose]; otherwise, <c>false</c>.</value>
        public bool AllowLose { get; set; }

        /// <summary>
        /// 允许GIF缩放带动画
        /// </summary>
        /// <value><c>true</c> if [allow GIF]; otherwise, <c>false</c>.</value>
        public bool AllowGif { get; set; }

        /// <summary>
        /// 缩放方式
        /// </summary>
        /// <value>The type of the resize.</value>
        public ResizeType ResizeType { get; set; }
    }

    /// <summary>
    /// Enum ResizeType
    /// </summary>
    public enum ResizeType
    {
        /// <summary>
        /// 固定宽高
        /// </summary>
        FixWidthAndHeight,
        /// <summary>
        /// 固定宽，自动计算高
        /// </summary>
        FixWidthAutoHeight,
        /// <summary>
        /// 固定高，自动计算宽
        /// </summary>
        FixHeightAutoWidth,
        /// <summary>
        /// 按大值计算
        /// </summary>
        ByMax,
        /// <summary>
        /// 按小值计算
        /// </summary>
        ByMin
    }
}