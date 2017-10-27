using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEnum
{

    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum EnumStatus
    {
        [Description("全部")]
        All = -1,
        [DescriptionAttribute("禁用")]
        Invalid = 0,
        [DescriptionAttribute("正常")]
        Valid = 1,
        [DescriptionAttribute("删除")]
        Delete = 9
    }
    /// <summary>
    /// 执行状态码
    /// </summary>
    public enum EnumResultStatus
    {
        [DescriptionAttribute("失败")]
        Failed = 0,
        [DescriptionAttribute("成功")]
        Success = 1,
        [DescriptionAttribute("无权限")]
        IsLimit = 10000,
        [DescriptionAttribute("系统错误")]
        Error = 10001,
        [DescriptionAttribute("存在数据")]
        Exists = 10002
    }
    public enum EnumDateType
    {
        Year = 1,
        Quarter = 2,
        Month = 3,
        Week = 4,
        Day = 5,
        Hour=6,
        Other=7
    }
    public enum EnumUserOperateType
    {
        [DescriptionAttribute("登录")]
        Login = 0,
        [DescriptionAttribute("发表日志")]
        SendLog = 1,
        [DescriptionAttribute("查看联系信息")]
        SeeLink = 2,
        [DescriptionAttribute("浏览用户")]
        SeeUser = 3,
        [DescriptionAttribute("其他")]
        Other = 4,
        [DescriptionAttribute("错误")]
        Error = 5,
        [DescriptionAttribute("购买")]
        Pay = 6,
        [DescriptionAttribute("后台管理")]
        Manage = 7,
    }
    public enum EnumSettingKey
    {
        /// <summary>
        /// 金币来源
        /// </summary>
        [DescriptionAttribute("IValue")]
        GoldSource = 1,
        /// <summary>
        /// 金额金币比例
        /// </summary>
        [DescriptionAttribute("DValue")]
        GoldScale = 2

    }
    public enum EnumPayType
    {
        [DescriptionAttribute("支付宝")]
        Alipy = 0,
        [DescriptionAttribute("线下汇款")]
        OffLine = 1,
        [DescriptionAttribute("银行卡")]
        BankCard = 2,
        [DescriptionAttribute("微信")]
        WXPay = 3
    }

    public enum EnumAccountType
    {
        [DescriptionAttribute("账户")]
        AccountFee = 0,
        [DescriptionAttribute("积分")]
        IntegerFee = 1,
        [DescriptionAttribute("优惠券")]
        DiscountFee = 2
    }

    public enum EnumPalyType
    {   
        [DescriptionAttribute("推广注册")]
        WelcomeIn = 0,
        [DescriptionAttribute("账户充值")]
        InAccount = 1,
        [DescriptionAttribute("账户提现")]
        OutAccount= 2,
        [DescriptionAttribute("提现失败")]
        OutAccountFault= 3,
        [DescriptionAttribute("投注扣款")]
        PlayNum = 4,
        [DescriptionAttribute("追号扣款")]
        ChaseNum=5,
        [DescriptionAttribute("追号返款")]
        ChaseNumBack=6,
        [DescriptionAttribute("游戏返点")]
        PlayNumBackPointe=7,
        [DescriptionAttribute("奖金派送")]
        RewardSend=8,
        [DescriptionAttribute("撤单返款")]
        ReturnPayNum=9,
        [DescriptionAttribute("撤单手续费")]
        ReturnBrokerage=10,
        [DescriptionAttribute("撤消返点")]
        ReturnBackPointe=11,
        [DescriptionAttribute("撤消派奖")]
        ReturnRewardSend=12,
        [DescriptionAttribute("充值扣费")]
        InAccountDeduction=13,
        [DescriptionAttribute("上级充值")]
        ParentPay=14,
        [DescriptionAttribute("活动礼金")]
        ActiveMoney=15,
        [DescriptionAttribute("其他")]
        Other=16,
        [DescriptionAttribute("下级返点")]
        ReturnBackByMember = 17,
        [DescriptionAttribute("工资发放")]
        WagesMoney = 18,
        [DescriptionAttribute("分红发放")]
        ProfitShareMoney = 19,
         [DescriptionAttribute("充值赠送")]
        PaySendAccount = 20,
         [DescriptionAttribute("投注赠送")]
        BettSendAccount = 21,
         [DescriptionAttribute("亏损赠送")]
        KSSendAccount = 22,
         [DescriptionAttribute("转账下级")]
         SendAccountToXJ = 23
    }
}
