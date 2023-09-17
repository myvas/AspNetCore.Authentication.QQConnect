# Myvas.AspNetCore.Authentication Family
* QQConnect: _this repo_
* WeixinOpen: [Here](https://github.com/myvas/AspNetCore.Authentication.WeixinOpen)
* WeixinAuth: [Here](https://github.com/myvas/AspNetCore.Authentication.WeixinAuth)

# What's this?
[![7.0](https://github.com/github/docs/actions/workflows/main.yml/badge.svg?branch=7.0)](https://github.com/myvas/AspNetCore.Authentication.QQConnect/tree/7.0)
[![6.0](https://github.com/github/docs/actions/workflows/main.yml/badge.svg?branch=6.0)](https://github.com/myvas/AspNetCore.Authentication.QQConnect/tree/6.0)
[![5.0](https://github.com/github/docs/actions/workflows/main.yml/badge.svg?branch=5.0)](https://github.com/myvas/AspNetCore.Authentication.QQConnect/tree/6.0)
[![3.1](https://github.com/github/docs/actions/workflows/main.yml/badge.svg?branch=release/3.1)](https://github.com/myvas/AspNetCore.Authentication.QQConnect/tree/release/3.1)

An ASP.NET Core authentication middleware for https://connect.qq.com (腾讯QQ互联平台/QQ登录）  
* 须腾讯QQ互联平台（connect.qq.com）账号。
* 用户可通过点击“QQ登录”图标按钮一键登入网站，或使用手机QQ程序扫码登入网站，当然，也可以输入QQ账号密码登入网站。

# How to Use?
## 0.Create account
在腾讯QQ互联平台(https://connect.qq.com)上创建应用（网站应用，移动应用），并指定网站回调地址（例如：https://www.myvas.com/signin-qqconnect)，记下AppId和AppKey。  

## 1.nuget
* [Myvas.AspnetCore.Authentication.QQConnect](https://www.nuget.org/packages/Myvas.AspNetCore.Authentication.QQConnect) 

## 2.Configure
```csharp
    app.UseAuthentication();
```

## 3.ConfigureServices
```csharp
services.AddAuthentication()
    // using Myvas.AspNetCore.Authentication;
    .AddQQConnect(options => 
    {
        options.AppId = Configuration["QQConnect:AppId"];
        options.AppKey = Configuration["QQConnect:AppKey"];

        options.CallbackPath = "/signin-qqconnect"; //default

        QQConnectScopes.TryAdd(options.Scope,
            QQConnectScopes.get_user_info,
            QQConnectScopes.list_album, //需要额外开通权限，暂未实现
            QQConnectScopes.upload_pic, //需要额外开通权限，暂未实现
            QQConnectScopes.do_like); //需要额外开通权限，暂未实现
    };
```

# Dev
* [.NET Core 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

# Demo Online
* [Here](https://demo.auth.myvas.com)
