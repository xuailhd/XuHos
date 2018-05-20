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
          "content": "{\"Data\":{\n          \"openid\":\"XXXXX\",\n          \"session_key\":\"XXX\",\n          \"unionid\":\"XXX\",\n           \"mobile\":\"15711112222\"  --有值得话代表已经绑定了手机号\n          },\"Total\":0,\"Status\":0,\"Msg\":\"\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/test/WechatAppController.cs",
    "groupTitle": "101_WechatAPP_Authorize",
    "name": "GetHttp39108180207WechatappAppauth"
  },
  {
    "type": "GET",
    "url": "http://39.108.180.207/WechatApp/BindMobile",
    "title": "101102/小程序绑定手机号",
    "group": "101_WechatAPP_Authorize",
    "description": "<p>如果校验Code，没有返回手机号，则绑定手机号</p>",
    "permission": [
      {
        "name": "已经校验Code"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "noncestr",
            "description": "<p>随机数，每次调用接口不能重复，长度10到40的字母或数字组成</p>"
          },
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "usertoken",
            "description": "<p>校验小程序Code 返回的 session_key</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "string",
            "optional": false,
            "field": "Mobile",
            "description": "<p>要绑定的手机号，测试阶段暂时不用验证码</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "{\n               \"Mobile\": \"XXXX\", \n}",
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
          "content": "{\"Data\":{\n          \"UserID\":\"XXXXX\",\n          },\"Total\":0,\"Status\":0,\"Msg\":\"\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/test/WechatAppController.cs",
    "groupTitle": "101_WechatAPP_Authorize",
    "name": "GetHttp39108180207WechatappBindmobile"
  }
] });
