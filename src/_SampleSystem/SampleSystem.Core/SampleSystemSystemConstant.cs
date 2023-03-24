using System;
using System.Collections.ObjectModel;

using Framework.Core;
using Framework.Configuration;

namespace SampleSystem;

public static class SampleSystemSystemConstant
{
    public const int DEFAULT_PHOTO_MAX_SIZE = 1024 * 1024;

    public const string JPEG_CONTENT_TYPE = "image/jpeg";

    public const string PNG_CONTENT_TYPE = "image/png";

    public static readonly SystemConstant<DateTime> SampleDateConstant = new SystemConstant<DateTime>(() => SampleDateConstant, DateTime.Now.ToStartMonthDate(), "SampleDateConstant");

    public static readonly SystemConstant<int> SampleInt32Constant = new SystemConstant<int>(() => SampleInt32Constant, 123, "SampleInt32Constant");

    public static readonly SystemConstant<string> SampleStringConstant = new SystemConstant<string>(() => SampleStringConstant, "HelloWorld", "SampleStringConstant");

    public static readonly ReadOnlyCollection<string> DEFAULT_PHOTO_CONTENT_TYPES = new[] { JPEG_CONTENT_TYPE, PNG_CONTENT_TYPE }.ToReadOnlyCollection();
}
