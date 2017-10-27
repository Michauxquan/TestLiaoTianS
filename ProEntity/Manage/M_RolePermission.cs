﻿/**  版本信息模板在安装目录下，可自行修改。
* RolePermission.cs
*
* 功 能： N/A
* 类 名： RolePermission
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/4/8 19:58:55   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace ProEntity.Manage
{
	/// <summary>
	/// RolePermission:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_RolePermission
	{
		public M_RolePermission()
		{}

		public int AutoID{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string RoleID{ get; set; }
		/// <summary>
		/// 
		/// </summary>
        public string MenuCode{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUserID{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string ClientID{ get; set; }

	}
}

