# Myvas.AspNetCore.Authentication Family
* QQConnect: this repo
* WeixinOpen: [Here](https://github.com/myvas/AspNetCore.Authentication.WeixinOpen)
* WeixinAuth: [Here](https://github.com/myvas/AspNetCore.Authentication.WeixinAuth)

# What's this?
An ASP.NET Core authentication middleware for https://connect.qq.com (腾讯QQ互联平台/QQ登录）  
腾讯QQ互联平台/QQ登录：须腾讯QQ互联平台（connect.qq.com）账号，用户通过点击“QQ登录”图标按钮，或使用手机QQ扫码登入网站。  

* nuget: [Myvas.AspnetCore.Authentication.QQConnect](https://www.nuget.org/packages/Myvas.AspNetCore.Authentication.QQConnect) 

# How to Use?
## 1.Create account
在腾讯QQ互联平台(https://connect.qq.com)上创建应用（网站应用，移动应用），并指定网站回调地址（例如：https://www.myvas.com/signin-qqconnect)，记下AppId和AppKey。  

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

        options.CallbackPath = "/signin-qqconnect"; //默认

        // using Myvas.AspNetCore.Authentication.QQConnect;
        QQConnectScopes.TryAdd(options.Scope,
            QQConnectScopes.get_user_info,
            QQConnectScopes.list_album, //需要额外开通权限，暂未实现
            QQConnectScopes.upload_pic, //需要额外开通权限，暂未实现
            QQConnectScopes.do_like); //需要额外开通权限，暂未实现
    };
```

# Dev
* [.NET Core 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [微信开发者工具](https://mp.weixin.qq.com/debug/wxadoc/dev/devtools/download.html)

# Demo Online
* Demo website: [Here](https://demo.auth.myvas.com)
* Demo source code: [Here](https://github.com/myvas/AspNetCore.Authentication.Demo)
