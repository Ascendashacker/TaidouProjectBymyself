using System.Text.RegularExpressions;
/// <summary>
/// 功能描述(Description)：	正则表达式
/// 作者(Author)：			Ascendashacker
/// 日期(Create Date)：		2019/2/18 15:31:39
/// </summary>
public class RegularUtils : Singleton<RegularUtils>
{
    #region 校验数字的表达式
    /// <summary>
    /// 是否是
    /// 非负数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool IsNonnegativeNumber(string input)
    {
        return IsRegexMatch(@"^\d+$|^\d+(\.\d+)?$", input);
    }
    #endregion

    #region 校验字符的表达式

    /// <summary>
    /// 是否是
    /// 英文和数字
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsEnglishOrNumber(string input)
    {
        return IsRegexMatch(@"^[A-Za-z0-9]+$", input);
    }

    /// <summary>
    /// 是否是
    /// 由数字、26个英文字母或者下划线组成的字符串
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsEnglishOrNumberOrUnderline(string input)
    {
        return IsRegexMatch(@"^\w+$", input);
    }

    /// <summary>
    /// 是否是
    /// 由中文、英文、数字包括下划线组成的字符串
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsChineseOrEnglishOrNumberOrUnderline(string input)
    {
        return IsRegexMatch(@"^[\u4E00-\u9FA5A-Za-z0-9_]+$", input);
    }
    /// <summary>
    /// 是否包含
    /// ^%&’,;=?$\”等字符
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsSpecialString(string input)
    {
        return IsRegexMatch(@"[^%&’,;=?$\x22]+", input);
    }

    /// <summary>
    /// 禁止输入含有~的字符
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsNoWaveLine(string input)
    {
        return IsRegexMatch(@"[^%&’,;=?$\x22]+", input);
    }

    #endregion

    #region 特殊需求表达式

    /// <summary>
    /// 是否是域名
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsDomainName(string input)
    {
        return IsRegexMatch(@"[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(/.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+/.?", input);
    }
    /// <summary>
    /// 是否是手机号码
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsTelePhoneNumber(string input)
    {
        return IsRegexMatch(@"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$", input);
    }
    /// <summary>
    /// 是否是电话号码
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsPhoneNumber(string input)
    {
        return IsRegexMatch(@"^($$\d{3,4}-)|\d{3.4}-)?\d{7,8}$", input);
    }
    /// <summary>
    /// 是否是身份证号(15位、18位数字)
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IdentityNumber(string input)
    {
        return IsRegexMatch(@"^\d{15}|\d{18}$", input);
    }

    /// <summary>
    /// 帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsLegalAccount(string input)
    {
        return IsRegexMatch(@"^[a-zA-Z][a-zA-Z0-9_]{4,15}$", input);
    }

    /// <summary>
    /// 密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线)
    /// </summary>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    public bool IsLegalPassword(string input)
    {
        return IsRegexMatch(@"^[a-zA-Z]\w{5,17}$", input);
    }
    #endregion

    /// <summary>
    /// 正则校验
    /// </summary>
    /// <param name="regexStr">正则</param>
    /// <param name="input">校验的字符串</param>
    /// <returns></returns>
    private bool IsRegexMatch(string regexStr, string input)
    {
        Regex regex = new Regex(regexStr);
        if (regex.IsMatch(input))
        {
            return true;
        }
        return false;
    }
}
