using FluentValidation;
using Hwj.SecretVault.Infra.Entity.ModuleAuthorization.Params;

namespace Hwj.SecretVault.Application.Validator.ModuleUserManagement
{
    /// <summary>
    /// 登录 Param 验证器
    /// </summary>
    public class JwtLoginParamValidator : AbstractValidator<JwtLoginParam>
    {
        public JwtLoginParamValidator()
        {
            RuleFor(param => param.Account).NotEmpty().WithMessage("账号不能为空").Length(6, 12).WithMessage("账号长度必须为[6,12]范围！");
            RuleFor(param => param.Password).NotEmpty().WithMessage("登录密码不能为空").Length(6, 12).WithMessage("密码长度必须为[6,12]范围！");
        }
    }
}