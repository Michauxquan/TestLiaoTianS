/**  版本信息模板在安装目录下，可自行修改。
* M_Users.cs
*
* 功 能： N/A
* 类 名： M_Users
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/3/6 19:52:53   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
namespace ProEntity.Manage
{
	/// <summary>
	/// M_Users:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Users 
	{
		public M_Users()
		{}


        public List<Menu> Menus { get; set; }
	    public decimal Rebate { get; set; }
        public decimal UsableRebate { get; set; }
	    public decimal AccountFee { get; set; }

	    /// <summary>
	    /// 
	    /// </summary>
	    public string UserID { get; set; }

	    /// 
		public int AutoID{ get; set; } 
		/// <summary>
		/// 
		/// </summary>
		public string LoginName{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string LoginPwd{get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string UserName{ get; set; }

        public string RoleID{ get; set; }
	    public int SourceType { get; set; }
	    public M_Role Role{ get; set; } 
		/// <summary>
		/// 
		/// </summary>
		public string Email{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string MobilePhone{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string OfficePhone{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Type{ get; set; }

	    public int IsLine { get; set; }

	    /// <summary>
		/// 
		/// </summary>
		public string Avatar{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? IsAdmin{ get; set; }

        public int? ChildCount { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Status{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Layers { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string AccountPwd{ get; set; }

	    /// <summary>
	    /// 
	    /// </summary>
	    public string Description { get; set; }

	    public DateTime? CreateTime{ get; set; }
        public DateTime? LastDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Property("Lower")] 
        public string CreateUserID{ get; set; }

        public M_Users CreateUser{ get; set; } 

        public string LastLoginIP { get; set; }


        public string RealName { get; set; }

        public int AskType { get; set; }

        public string AnswerContent { get; set; } 

        public string PrevLoginIP { get; set; }

	    public int rgzStatus { get; set; }
        public int fhStatus { get; set; }
        public decimal fhPoint { get; set; }

	    public int Backid { get; set; }
        public decimal SSCNum{get;set;}
        public decimal SSCUseNum{get;set;}
        public decimal FCNum{get;set;}
        public decimal FCUseNum{get;set;}
        public decimal X5Num{get;set;}
        public decimal X5UseNum{get;set;}
        public decimal SSCType{get;set;}
        public decimal FCType{get;set;}
        public decimal X5Type{get;set;}
        public decimal WinFee {get;set;}

	    public int IsRestBank { get; set; }

	    /// <summary>
        /// 等级
        /// </summary>
	    public int SafeLevel { get; set; }   
        public DateTime? UpdateTime { get; set; }  
	    /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }

	}
}

