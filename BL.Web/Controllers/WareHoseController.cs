﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.MVC;
using BL.Service;
using BL.Models;
using BL.Framework;

namespace BL.Web.Controllers
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class WareHoseController : Controller
    {
        WareHoseService wareHose = new WareHoseService();
        public ActionResult LoadWareHose()
        {
            return View();
        }
        /// <summary>
        /// hpf 获取所有仓库信息
        /// </summary>
        /// <returns></returns>
        [JsonException]
        public string GetAllWareHoseJson()
        {
            var lst= wareHose.GetAllWareHoseInfo();
            return JsonHelper.Instance.Serialize(new { list = lst, pageSize=lst.Count() });
        }
        /// <summary>
        /// 修改或添加仓库信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [JsonException]
        public string EditWareHoseJson(string json)
        {
            var models = JsonHelper.Instance.Deserialize<List<T_WAREHOUSEModel>>(json);
            var model = models[0];
            model.FCREATEID = UserContext.CurrentUser.UserName;
            if (model.FCATEGORY == "1")
            {
                model.FPARENTID = "-1";
            }
            if (string.IsNullOrEmpty(model.FGUID))//没有guid表示新增
            {
                model.FGUID = Guid.NewGuid().ToString();
                model.FCREATETIME = DateTime.Now;
                model.FSTATUS = "1";
                var result = wareHose.AddWareHoseInfo(model);
                return JsonHelper.Instance.Serialize(result);
            }
            else
            {
                var result = wareHose.EditWareHoseByGuid(model);
                return JsonHelper.Instance.Serialize(result);
            }
        }
        /// <summary>
        /// 删除仓库信息
        /// </summary>
        /// <param name="FGUID"></param>
        /// <returns></returns>
        [JsonException]
        public string DelWareHoseJson(string FGUID)
        {
            if (wareHose.DelWareHoseByGuid(FGUID))
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse { closeCurrent = true });
            }else
            {
                return JsonHelper.Instance.Serialize(new Models.UiResponse {statusCode="300", closeCurrent = true ,message="删除失败"});
            }
        }
        /// <summary>
        /// 设置仓库状态
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FSTATUS">1未启用，2已启用，3禁用</param>
        /// <returns></returns>
        [JsonException]
        public string SetWareHoseStatus(string FGUID,string FSTATUS)
        {
            var time = DateTime.Now;
            return JsonHelper.Instance.Serialize(new {result= wareHose.SetWareHoseStatusByGuid(FGUID, FSTATUS, time), data=FSTATUS,time=time });
        }
    }
}
