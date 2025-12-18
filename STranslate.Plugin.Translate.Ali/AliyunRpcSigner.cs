using System.Security.Cryptography;
using System.Text;

namespace STranslate.Plugin.Translate.Ali;

public static class AliyunRpcSigner
{
    public static string Sign(
        IDictionary<string, string> parameters,
        string accessKeySecret,
        string httpMethod = "POST")
    {
        // 1. 排序 + URL Encode
        var sorted = parameters
            .OrderBy(p => p.Key, StringComparer.Ordinal)
            .Select(p =>
                $"{PercentEncode(p.Key)}={PercentEncode(p.Value)}");

        var canonicalizedQueryString = string.Join("&", sorted);

        // 2. StringToSign
        var stringToSign =
            $"{httpMethod}&%2F&{PercentEncode(canonicalizedQueryString)}";

        // 3. HMAC-SHA1
        using var hmac = new HMACSHA1(
            Encoding.UTF8.GetBytes(accessKeySecret + "&"));

        var hash = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(stringToSign));

        return Convert.ToBase64String(hash);
    }

    private static string PercentEncode(string value)
    {
        return Uri.EscapeDataString(value)
            .Replace("+", "%20")
            .Replace("*", "%2A")
            .Replace("%7E", "~");
    }
}