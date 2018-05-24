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
    "filename": "XuHos.Service.WebApi/Controllers/Platform/WechatAppController.cs",
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
    "filename": "XuHos.Service.WebApi/Controllers/Platform/WechatAppController.cs",
    "groupTitle": "101_WechatAPP_Authorize",
    "name": "GetHttp39108180207WechatappBindmobile"
  },
  {
    "type": "GET",
    "url": "/Doctors",
    "title": "102103/查询医生列表",
    "group": "102_Personal_Info",
    "description": "<p>通过关键字查询医生列表</p>",
    "permission": [
      {
        "name": "所有人"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "CurrentPage",
            "defaultValue": "1",
            "description": "<p>页码</p>"
          },
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "PageSize",
            "defaultValue": "10",
            "description": "<p>分页大小</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": false,
            "field": "Keyword",
            "description": "<p>关键字</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": false,
            "field": "OrderBy",
            "defaultValue": "排序规则，可叠加（4：按照有无排班排序；5：按照有无套餐排序；6：按照评分降序排序；7：按问诊回复量降序排序）",
            "description": ""
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?CurrentPage=1&PageSize=10&Keyword=&OrderBy=4&OrderBy=6",
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
            "description": "<p>业务数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n                \"Data\": [\n                    {\n                        \"DoctorID\": \"4FDADA2DD7E3450CAEC78E9CA407BF06\",\n                        \"DoctorName\": \"向金龙\",\n                        \"Gender\": 0,\n                        \"Marriage\": 0,\n                        \"Birthday\": \"19850808\",\n                        \"IDType\": 0,\n                        \"Address\": \"深圳市福田区国际创新中心A座8楼\",\n                        \"IsConsultation\": false,\n                        \"IsExpert\": false,\n                        \"areaCode\": \"\",\n                        \"HospitalID\": \"42FF1C61132E443F862510FF3BC3B03A\",\n                        \"HospitalName\": \"康美医院\",\n                        \"DepartmentID\": \"BCE87580389041A0A70F9465F305BBC2\",\n                        \"DepartmentName\": \"全科\",\n                        \"Duties\": \"\",\n                        \"CheckState\": 0,\n                        \"Sort\": 0,\n                        \"DoctorType\" : 1, //医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生\n                        \"IsScheduleExist\": true, //是否有排班\n                        \"IsPackageExist\": true, //是否可使用套餐\n                        \"ReplyCount\" : 99, //最近一周的回复数（图文以及音视频）\n                        \"EvaluationScore\": 9.9, // 服务综合评分\n                        \"DoctorServices\": [\n                            {\n                                \"ServiceType\": 3,//视频咨询\n                                \"ServiceSwitch\": 1,//开启\n                                \"ServicePrice\": 8//价格（单位：元）\n                            },\n                            {\n                                \"ServiceType\": 2,//语音咨询\n                                \"ServiceSwitch\": 1,\n                                \"ServicePrice\": 5\n                            },\n                            {\n                                \"ServiceType\": 4,//家庭医生\n                                \"ServiceSwitch\": 1,\n                                \"ServicePrice\": 7\n                            },\n                            {\n                                \"ServiceType\": 1,//图文咨询\n                                \"ServiceSwitch\": 1,\n                                \"ServicePrice\": 1\n                            }\n                        ],\n                        \"User\": {\n                            \"UserID\": \"5E5E4318744248E99C18A71B8774E2E9\",\n                            \"UserType\": 0,\n                            \"PhotoUrl\": \"http://www.kmwlyy.com/Uploads/doctor/xiangjinlong.jpg\",\n                        }\n                    }\n                ],\n                \"Total\": 75,\n                \"Status\": 0,\n                \"Msg\": \"操作成功\"\n            }",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorsController.cs",
    "groupTitle": "102_Personal_Info",
    "name": "GetDoctors"
  },
  {
    "type": "GET",
    "url": "/Doctors/GetDoctorInfo",
    "title": "102101/获取医生个人信息",
    "group": "102_Personal_Info",
    "version": "4.0.0",
    "description": "<p>获取个人资料 作者：郭超</p>",
    "permission": [
      {
        "name": "用户登录"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "apptoken",
            "description": "<p>Users unique access-key.</p>"
          },
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          },
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "sign",
            "description": "<p>apptoken=@apptoken&amp;noncestr=@noncestr&amp;usertoken=@userToken&amp;appkey=@ appkey 串MD5加密后转成大写</p>"
          }
        ]
      }
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
            "description": "<p>总记录</p>"
          },
          {
            "group": "Response",
            "type": "object",
            "optional": false,
            "field": "Data",
            "description": "<p>业务数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n   \"Data\":{\"Intro\":\"...\",\"PhotoUrl\":\"...\",\"PhotoUrl\":\"\"},\n   \"Total\":0,\n   \"Status\":0,\n   \"Msg\":\"获取信息成功\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorsController.cs",
    "groupTitle": "102_Personal_Info",
    "name": "GetDoctorsGetdoctorinfo"
  },
  {
    "type": "GET",
    "url": "/Doctors/?ID=:ID",
    "title": "102104/获取医生详情",
    "group": "102_Personal_Info",
    "version": "4.0.0",
    "description": "<p>获取医生详情</p>",
    "permission": [
      {
        "name": "所有人"
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
            "description": "<p>userToken，用户未登录时传空</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "ID",
            "description": "<p>医生编号</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?ID=89F9E5907FD04DBF96A9867D1FA30396",
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
            "description": "<p>总记录</p>"
          },
          {
            "group": "Response",
            "type": "object",
            "optional": false,
            "field": "Data",
            "description": "<p>业务数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n                \"Data\": {\n                    \"DoctorID\": \"89F9E5907FD04DBF96A9867D1FA30396\",\n                    \"DoctorName\": \"邱浩强\",\n                    \"IsFollowed\":false,//是否已关注\n                    \"UserID\": \"B04D4AE28F994AE2AACBB456D7E0647B\",\n                    \"Gender\": 1,\n                    \"Marriage\": 0,\n                    \"Birthday\": \"19850808\",\n                    \"IDType\": 4,\n                    \"IDNumber\": \"123\",\n                    \"CertificateNo\": \"89F9E5907FD04DBF96A9867D1FA30396\",\n                    \"Address\": \"2131\",\n                    \"PostCode\": \"231\",\n                    \"Intro\": \"... ...\",\n                    \"IsConsultation\": false,\n                    \"IsExpert\": false,\n                    \"IsFreeClinicr\": false,\n                    \"Specialty\": \"高血压 糖尿病 恶性肿瘤 其他\",\n                    \"areaCode\": \"\",\n                    \"HospitalID\": \"42FF1C61132E443F862510FF3BC3B03A\",\n                    \"HospitalName\": \"康美医院\",\n                    \"Grade\": \"0\",\n                    \"DepartmentID\": \"A8064D2DAE3542B18CBD64F467828F57\",\n                    \"DepartmentName\": \"健康体检中心\",\n                    \"Education\": \"\",\n                    \"Title\": \"4\",\n                    \"Duties\": \"\",\n                    \"CheckState\": 1,\n                    \"SignatureURL\": \"\",\n                    \"Sort\": 0,\n                    \"FollowNum\": 4,//关注量\n                    \"DiagnoseNum\": 3,\n                    \"ConsultNum\": 1,\n                    \"ConsulServicePrice\": 0,\n                    \"Department\": {\n                        \"DepartmentID\": \"A8064D2DAE3542B18CBD64F467828F57\",\n                        \"HospitalID\": \"42FF1C61132E443F862510FF3BC3B03A\",\n                        \"DepartmentName\": \"健康体检中心\",\n                        \"Intro\": \"...\"\n                    },\n                    \"DoctorServices\": [\n                        {\n                            \"ServiceID\": \"a55291ac95b4472ba1c966953fc17b49\",\n                            \"DoctorID\": \"89F9E5907FD04DBF96A9867D1FA30396\",\n                            \"ServiceType\": 3,\n                            \"ServiceSwitch\": 1,\n                            \"ServicePrice\": 0.01,\n                            \"HasSchedule\": true\n                        }\n                    ],\n                    \"Hospital\": {\n                        \"HospitalID\": \"42FF1C61132E443F862510FF3BC3B03A\",\n                        \"HospitalName\": \"康美医院\",\n                        \"Intro\": \"...\",\n                        \"License\": \"YYZZ000001\",\n                        \"LogoUrl\": \"http://121.15.153.63:8028///Uploads/hospital/201509/151815495260.png\",\n                        \"Address\": \"广东省普宁市流沙新河西路38号\",\n                        \"PostCode\": \"515300\",\n                        \"Telephone\": \"(0663)2229222\",\n                        \"Email\": \"km@kmlove.com.cn\",\n                        \"ImageUrl\": \"http://121.15.153.63:8028///Uploads/hospital/201512/1.jpg\"\n                    },\n                    \"User\": {\n                        \"UserID\": \"B04D4AE28F994AE2AACBB456D7E0647B\",\n                        \"UserAccount\": \"jack\",\n                        \"UserCNName\": \"邱浩强\",\n                        \"UserENName\": \"3\",\n                        \"UserType\": 2,\n                        \"Mobile\": \"13692248249\",\n                        \"Email\": \"zenglu@km.com\",\n                        \"PayPassword\": \"\",\n                        \"PhotoUrl\": \"http://121.15.153.63:8028/images/b427cae4799bf5387eadfc9d7e627e2e\",\n                        \"Score\": 0,\n                        \"Star\": 0,\n                        \"Comment\": 0,\n                        \"Good\": 0,\n                        \"Fans\": 0,\n                        \"Grade\": 0,\n                        \"Checked\": 0,\n                        \"RegTime\": \"2016-08-01T17:40:07.92\",\n                        \"CancelTime\": \"2016-08-01T17:40:07.92\",\n                        \"UserState\": 0,\n                        \"UserLevel\": 3,\n                        \"Terminal\": \"0\",\n                        \"LastTime\": \"2016-12-26T13:57:08.013\",\n                        \"identifier\": 110\n                    }\n                },\n                \"Total\": 0,\n                \"Status\": 0,\n                \"Msg\": \"操作成功\"\n            }",
          "type": "json"
        }
      ]
    },
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorsController.cs",
    "groupTitle": "102_Personal_Info",
    "name": "GetDoctorsIdId"
  },
  {
    "type": "Post",
    "url": "/Doctors/UpdateDoctorInfo",
    "title": "102102/更新医生信息",
    "group": "102_Personal_Info",
    "description": "<p>更新医生信息</p>",
    "permission": [
      {
        "name": "医生登陆"
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
            "description": "<p>userToken，用户未登录时传空</p>"
          },
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "sign",
            "description": "<p>apptoken=@apptoken&amp;noncestr=@noncestr&amp;usertoken=@userToken&amp;appkey=@ appkey 串MD5加密后转成大写</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Intro",
            "description": "<p>个人介绍</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "Specialty",
            "description": "<p>擅长领域</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "PhotoUrl",
            "description": "<p>头像</p>"
          }
        ]
      }
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
            "description": "<p>总记录</p>"
          },
          {
            "group": "Response",
            "type": "object[]",
            "optional": false,
            "field": "Data",
            "description": "<p>业务数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\"Data\":True,\"Total\":0,\"Status\":0,\"Msg\":\"操作成功\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorsController.cs",
    "groupTitle": "102_Personal_Info",
    "name": "PostDoctorsUpdatedoctorinfo"
  },
  {
    "type": "Get",
    "url": "/Doctors/GetMyVisitDoctors",
    "title": "104001/已就诊的医生",
    "group": "104_Treatment",
    "description": "<p>查询已就诊过的医生</p>",
    "permission": [
      {
        "name": "已登录(用户)"
      }
    ],
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "apptoken",
            "description": "<p>Users unique access-key.</p>"
          },
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          },
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "sign",
            "description": "<p>apptoken=@apptoken&amp;noncestr=@noncestr&amp;usertoken=@userToken&amp;appkey=@ appkey 串MD5加密后转成大写</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "CurrentPage",
            "defaultValue": "1",
            "description": "<p>页码</p>"
          },
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "PageSize",
            "defaultValue": "10",
            "description": "<p>分页大小</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?CurrentPage=1&PageSize=10",
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
            "description": "<p>总记录</p>"
          },
          {
            "group": "Response",
            "type": "object",
            "optional": false,
            "field": "Data",
            "description": "<p>业务数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n           \"Data\": [\n               {\n                   \"OPDRegisterID\": \"90ecac5ebf2d4587876733155af46c6d\",\n                   \"DoctorID\": \"2bc4be23a01f4c7c865671918721df1d\",\n                   \"DoctorName\": \"吴大大\",\n                   \"HospitalID\": \"c4cd1db578d84b94bd091f95495f256a\",\n                   \"HospitalName\": \"His对接医院(勿删)\",\n                   \"DepartmentName\": \"测试-外科\",\n                   \"DepartmentID\": \"c3d5f41a930b4412ba57cc1ad11e871e\",\n                   \"Gender\": \"\",\n                   \"Portait\": \"http://121.15.153.63:8028/images/doctor/default.jpg\",\n                   \"Position\": \"\",\n                   \"IsExpert\": false,\n                   \"Specialties\": \"\",\n                   \"Title\": \"\",\n                   \"IsFollowed\": false,\n                   \"DoctorType\" : 1, //医生类型 0-互联网医生 1-多点执业医生 2-执业医生(在康美医院工作的) 3-自聘医生\n               }\n           ],\n           \"Total\": 1,\n           \"Status\": 0,\n           \"Msg\": \"操作成功\"\n       }",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorsController.cs",
    "groupTitle": "104_Treatment",
    "name": "GetDoctorsGetmyvisitdoctors"
  },
  {
    "type": "GET",
    "url": "/DoctorSchedule/GetDoctorScheduleList",
    "title": "106101/获取医生排班设置",
    "group": "106_Doctor_Setting",
    "description": "<p>用于获取医生排班设置</p>",
    "permission": [
      {
        "name": "登录"
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
            "type": "Date",
            "optional": false,
            "field": "beginDate",
            "description": "<p>查询起始日期</p>"
          },
          {
            "group": "Parameter",
            "type": "Date",
            "optional": false,
            "field": "endDate",
            "description": "<p>查询结束日期,如果超过起始日期3个月，只返回3个月</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例:",
          "content": "?beginDate=2018-05-24&endDate=2018-06-24   //参数可能需要url编码",
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
            "description": "<p>总记录</p>"
          },
          {
            "group": "Response",
            "type": "object",
            "optional": false,
            "field": "Data",
            "description": "<p>业务数据，会按照 日期，StartTime排序</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n   \"Data\":[{\n     \"OPDate\":\"2016-08-15\",\n     \"StartTime\":\"01:00\",   \n     \"EndTime\":\"02:00\",\n    \"Checked\":true},\n    {\n     \"OPDate\":\"2016-08-15\",\n     \"StartTime\":\"02:00\",\n     \"EndTime\":\"03:00\",\n    \"Checked\":true}\n   ],\n   \"Total\":0,\n   \"Status\":0,\n   \"Msg\":\"保存成功\"\n}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorScheduleController.cs",
    "groupTitle": "106_Doctor_Setting",
    "name": "GetDoctorscheduleGetdoctorschedulelist"
  },
  {
    "type": "POST",
    "url": "/DoctorSchedule/AddDoctorSchduleList",
    "title": "106102/保存排班设置",
    "group": "106_Doctor_Setting",
    "description": "<p>用于批量保存医生排班设置</p>",
    "permission": [
      {
        "name": "登录"
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
            "type": "String",
            "optional": false,
            "field": "Data",
            "description": "<p>批量保存排班数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例:",
          "content": "[{\n   \"OPDate\":\"2016-08-15\",\n   \"StartTime\":\"01:00\",   时间段范围必须是从系统获取的排版设置的时间段，不是自己去定义的。\n   \"EndTime\":\"02:00\",\n  \"Checked\":true},\n  {\n   \"OPDate\":\"2016-08-15\",\n   \"StartTime\":\"02:00\",\n   \"EndTime\":\"03:00\",\n  \"Checked\":true}\n ]",
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
            "description": "<p>总记录</p>"
          },
          {
            "group": "Response",
            "type": "object",
            "optional": false,
            "field": "Data",
            "description": "<p>业务数据</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n   \"Data\":null,\n   \"Total\":0,\n   \"Status\":0,\n   \"Msg\":\"保存成功\"\n}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Doctor/DoctorScheduleController.cs",
    "groupTitle": "106_Doctor_Setting",
    "name": "PostDoctorscheduleAdddoctorschdulelist"
  },
  {
    "type": "GET",
    "url": "/Cashier/AliPay",
    "title": "114001/支付宝预支付",
    "group": "114_Payment",
    "description": "<p>获取云通信独立认证配置</p>",
    "permission": [
      {
        "name": "已登录"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
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
            "field": "OrderNo",
            "description": "<p>订单编号</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": true,
            "field": "SellerID",
            "defaultValue": "wxf1b0cceac4c331e3",
            "description": "<p>收款账号</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": true,
            "field": "SignType",
            "defaultValue": "0",
            "description": "<p>签名类型（APP=0,Web=1,Wap=2）</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": true,
            "field": "ReturnUrl",
            "description": "<p>返回地址</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?OrderNo=42FF1C61132E443F862510FF3BC3B03A",
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
            "description": "<p>配置信息</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n            \"Data\": \"_input_charset=utf-8&notify_url=http%3a%2f%2fwww.kmwlyy.com%2f%2fCashier%2fAliPay%NotifyUrl&out_trade_no=KM2016081214142447185198&partner=2088021337472610&payment_type=1&return_url=http%3a%2f%2fwww.kmwlyy.com%2f%2fCashier%2fAliPay%2fReturnUrl&seller_id=2088021337472610&service=create_direct_pay_by_user&total_fee=2.76&sign=696f5a2a70356988a384259323b7635e&sign_type=MD5\",\n            \"Total\": 0,\n            \"Status\": 0,\n            \"Msg\": \"操作成功\"\n        }",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Platform/Cashier/AliPay/AliPayController.cs",
    "groupTitle": "114_Payment",
    "name": "GetCashierAlipay"
  },
  {
    "type": "GET",
    "url": "/Cashier/WxPay",
    "title": "114401/微信预支付",
    "group": "114_Payment",
    "version": "4.0.0",
    "description": "<p>微信预支付</p>",
    "permission": [
      {
        "name": "已登录"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
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
            "field": "OrderNo",
            "description": "<p>订单编号</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": true,
            "field": "SellerID",
            "defaultValue": "wxf1b0cceac4c331e3",
            "description": "<p>收款账号</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": true,
            "field": "SignType",
            "defaultValue": "0",
            "description": "<p>签名类型（APP=0,Web=1,Wap=2,JS=3）</p>"
          },
          {
            "group": "Parameter",
            "type": "string",
            "optional": true,
            "field": "OpenId",
            "description": "<p>用户编号</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?OrderNo=42FF1C61132E443F862510FF3BC3B03A",
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
            "description": "<p>配置信息</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n                \"Data\": {\n                    \"appId\": \"wx7833444576404bcb\",  \n                    \"timeStamp\":\"134134132413\",\n                    \"package\":\"Sign=WXPay\",\n                    \"prepay_id\": \"wx2016082211541365c425a5bc0232230911\",\n                    \"nonce_str\": \"ECb1tzPtvX1FcKU8\",\n                    \"partnerId\": \"1263997401\",\n                    \"sign\": \"923EDEF1288CD891E95D288DCCB3B5B4\",\n                },\n                \"Total\": 0,\n                \"Status\": 0,\n                \"Msg\": \"操作成功\"\n        }",
          "type": "json"
        }
      ]
    },
    "filename": "XuHos.Service.WebApi/Controllers/Platform/Cashier/WxPay/WxPayController.cs",
    "groupTitle": "114_Payment",
    "name": "GetCashierWxpay"
  },
  {
    "type": "GET",
    "url": "/IM/Config",
    "title": "117001/获取云通信配置",
    "group": "117_IM",
    "description": "<p>获取云通信独立认证配置</p>",
    "permission": [
      {
        "name": "已登录"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          }
        ]
      }
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
            "description": "<p>配置信息</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\"Data\":{\n            \"sdkAppID\":1400009922,\n            \"userSig\":\"eJx1jktTgzAURvf9FQxbHQcCSYg7ykuqVdFW1A1DIUhaHjEElHH871qmM7Lxbs*Z*52vhaIo6ubm8SLNsrZvZCJHTlXlUlEh0IF6-sc5Z3mSysQQ*cR1U-s9QsDcop*cCZqkhaRisix01GYGy2kjWcFOnNimYxkEIQdbpuvCpefbyMIIQaLbAM9-d-khmSr*n*-Y2wTX3osTRq7hlbdtP5Bqt6rqYPuxK3j5ui39O9qvARmcbj8GeXxVCxGFpX0ftQPizfs1Jv5mhMi3xrA*LB8qR0YrMIg8jquns*cg2*PZpGQ1PQVhDQMLaXBGByo61jaTADQd6sA4Zmvq4nvxAyg3ZbU_\",\n            \"identifier\":\"123\",\n            \"accountType\":5212\n            },\"Total\":0,\"Status\":0,\"Msg\":\"\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Platform/IMController.cs",
    "groupTitle": "117_IM",
    "name": "GetImConfig"
  },
  {
    "type": "GET",
    "url": "/IM/GetRoomInfo",
    "title": "117007/获取房间信息",
    "group": "117_IM",
    "description": "<p>获取房间信息</p>",
    "permission": [
      {
        "name": "已登录（用户/医生/分诊医生）"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "ChannelID",
            "description": "<p>房间编号</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?ChannelID=XXXX",
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
            "type": "int",
            "optional": false,
            "field": "Data",
            "description": "<p>预约信息</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\n                \"Data\": {\n                    \"ConversationRoomID\": \"650de17bbd184c1fae87b1bb5386ceb7\",\n                    \"ServiceID\": \"8f8c435e09e24ce2af61d92fa2ff422e\",\n                    \"ChannelID\": 9,\n                    \"Secret\": \"593c3fe883234f9ea2e7d6b83ad0c7c8\",\n                    \"RoomState\": 2,   0=未就诊,1=候诊中,2=就诊中,3=已就诊,4=呼叫中,5=离开中\n                    \"BeginTime\": \"2016-08-18T13:42:35.8\",\n                    \"EndTime\": \"2016-08-18T09:00:57.6001987\",\n                    \"TotalTime\": 0\n                },\n                \"Total\": 0,\n                \"Status\": 0,\n                \"Msg\": \"操作成功\"\n            }",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Platform/IMController.cs",
    "groupTitle": "117_IM",
    "name": "GetImGetroominfo"
  },
  {
    "type": "GET",
    "url": "/IM/MediaConfig",
    "title": "117002/获取多媒体配置",
    "group": "117_IM",
    "description": "<p>获取多媒体配置（视频直播、录制需要）</p>",
    "permission": [
      {
        "name": "已登录"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "ChannelID",
            "description": "<p>房间编号</p>"
          },
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "Identifier",
            "description": "<p>当前用户唯一标识（通过IM/Config接口获取）</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "?ChannelID=XXX&Identifier=1",
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
            "description": "<p>配置信息</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\"Data\":{\n           \"MediaChannelKey\":\"XXXX\",//进入视频是使用的动态秘钥\n           \"RecordingKey\":\"XXXX\",\n           \"Secret\":\"XXX\", //进入视频房间时的动态密码\n            \"Duration\":120,//服务时长\n            \"TotalTime\":10,//已消耗\n           },\"Total\":0,\"Status\":0,\"Msg\":\"\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Platform/IMController.cs",
    "groupTitle": "117_IM",
    "name": "GetImMediaconfig"
  },
  {
    "type": "POST",
    "url": "/IM/SetRoomState",
    "title": "117008/修改房间状态",
    "group": "117_IM",
    "description": "<p>修改房间状态</p>",
    "permission": [
      {
        "name": "已登录"
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
            "description": "<p>登录用户token，用户未登录时传空</p>"
          }
        ]
      }
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "ChannelID",
            "description": "<p>房间编号</p>"
          },
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "State",
            "description": "<p>新的状态  0=未就诊,1=候诊中,2=就诊中,3=已就诊,4=呼叫中,5=离开中</p>"
          },
          {
            "group": "Parameter",
            "type": "int",
            "optional": false,
            "field": "ExpectedState",
            "description": "<p>当前状态 0=未就诊,1=候诊中,2=就诊中,3=已就诊,4=呼叫中,5=离开中</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "请求样例：",
          "content": "{\n   ChannelID:\"XXX\",\n   State:2,\n   ExpectedState:1\n}",
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
            "type": "bool",
            "optional": false,
            "field": "Data",
            "description": "<p>是否成功</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "返回样例:",
          "content": "{\"Data\":true,\"Total\":2,\"Status\":0,\"Msg\":\"\"}",
          "type": "json"
        }
      ]
    },
    "version": "0.0.0",
    "filename": "XuHos.Service.WebApi/Controllers/Platform/IMController.cs",
    "groupTitle": "117_IM",
    "name": "PostImSetroomstate"
  }
] });
