define({ "api": [
  {
    "type": "GET",
    "url": "http://39.108.180.207/WechatApp/AppAuth",
    "title": "101101/校验小程序Code",
    "group": "101_WechatAPP_Authorize",
    "description": "<p>校验小程序Code</p>",
    "permission": [
      {
        "name": "所有人"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "string",
            "optional": false,
            "field": "code",
            "description": "<p>临时登录凭证</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "/WechatApp/AppAuth?code=xxxx",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Response": [
          {
            "group": "Response",
            "type": "String",
            "optional": false,
            "field": "Msg",
            "description": "<p>提示信息</p>"
          },
          {
            "group": "Response",
            "type": "int",
            "optional": false,
            "field": "Status",
            "description": "<p>0 代表无错误 1代表有错误</p>"
          },
          {
            "group": "Response",
            "type": "int",
            "optional": false,
            "field": "Total",
            "description": "<p>总记录数</p>"
          },
          {
            "group": "Response",
            "type": "Array",
            "optional": false,
            "field": "Data",
            "description": "<p>数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\"Data\":{\n          \"openid\":\"XXXXX\",\n          \"session_key\":\"XXX\",\n          \"unionid\":\"XXX\",\n          },\"Total\":0,\"Status\":0,\"Msg\":\"\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/test/WechatAppController.cs",
    "groupTitle": "101_WechatAPP_Authorize",
    "name": "GetHttp39108180207WechatappAppauth"
  }
] });
