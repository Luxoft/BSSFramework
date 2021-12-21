using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net.Mime;

using Framework.Core;

namespace Framework.Graphviz
{
    public class GraphvizFormatMetadata
    {
        private GraphvizFormatMetadata(ImageFormat imageFormat)
            : this(imageFormat.GetCodecInfo())
        {

        }

        private GraphvizFormatMetadata(ImageCodecInfo imageCodecInfo)
            : this(imageCodecInfo.GetExtension(), imageCodecInfo.MimeType)
        {

        }


        private GraphvizFormatMetadata(string extension, string contentType)
        {
            this.Extension = extension;
            this.ContentType = contentType;
        }


        public string Extension { get; private set; }

        public string ContentType { get; private set; }


        public static GraphvizFormatMetadata FromFormat(GraphvizFormat imageFormat)
        {
            return OutputFormats[imageFormat];
        }

        public static GraphvizFormatMetadata FromFormat(string imageFormat)
        {
            return OutputFormats[EnumHelper.Parse<GraphvizFormat>(imageFormat)];
        }

        public static GraphvizFormatMetadata FromFormat(ImageFormat imageFormat)
        {
            return FromFormat(imageFormat.ToString());
        }


        private static readonly Dictionary<GraphvizFormat, GraphvizFormatMetadata> OutputFormats = new Dictionary<GraphvizFormat, GraphvizFormatMetadata>
        {
            //Raster image formats
            { GraphvizFormat.Png, new GraphvizFormatMetadata(ImageFormat.Png) },
            { GraphvizFormat.Bmp, new GraphvizFormatMetadata(ImageFormat.Bmp) },
            { GraphvizFormat.Gif, new GraphvizFormatMetadata(ImageFormat.Gif) },
            { GraphvizFormat.Jpeg, new GraphvizFormatMetadata(ImageFormat.Jpeg) },
            { GraphvizFormat.Tiff, new GraphvizFormatMetadata(ImageFormat.Tiff) },

            //Vector etc
            { GraphvizFormat.Svg, new GraphvizFormatMetadata("svg", "image/svg+xml") },
            { GraphvizFormat.SvgZ, new GraphvizFormatMetadata("svgz", "image/svg+xml") },
            { GraphvizFormat.Vml, new GraphvizFormatMetadata("vml", "image/vml+xml") },
            { GraphvizFormat.VmlZ, new GraphvizFormatMetadata("vmlz", "image/vml+xml") },
            { GraphvizFormat.Pdf, new GraphvizFormatMetadata("pdf", MediaTypeNames.Application.Pdf) },
        };


        public static readonly GraphvizFormatMetadata Png = OutputFormats[GraphvizFormat.Png];

        public static readonly GraphvizFormatMetadata Bmp = OutputFormats[GraphvizFormat.Bmp];

        public static readonly GraphvizFormatMetadata Gif = OutputFormats[GraphvizFormat.Gif];

        public static readonly GraphvizFormatMetadata Jpeg = OutputFormats[GraphvizFormat.Jpeg];

        public static readonly GraphvizFormatMetadata Tiff = OutputFormats[GraphvizFormat.Tiff];

        public static readonly GraphvizFormatMetadata Svg = OutputFormats[GraphvizFormat.Svg];

        public static readonly GraphvizFormatMetadata SvgZ = OutputFormats[GraphvizFormat.SvgZ];

        public static readonly GraphvizFormatMetadata Vml = OutputFormats[GraphvizFormat.Vml];

        public static readonly GraphvizFormatMetadata VmlZ = OutputFormats[GraphvizFormat.VmlZ];

        public static readonly GraphvizFormatMetadata Pdf = OutputFormats[GraphvizFormat.Pdf];
    }
}