using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IgniteSlideExpress.UI.Shared;

public static class HtmlExtensions
{
    public static HtmlString DisabledIf(this HtmlHelper html, bool condition)
    {
        return new HtmlString(condition ? "disabled=\"disabled\"" : "");
    }
}