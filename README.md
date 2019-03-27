# AspNetCore.Authentication.QQConnect
An ASP.NET Core authentication middleware: QQConnect for https://connect.qq.com (腾讯QQ互联/QQ登录）

腾讯QQ互联/QQ登录：用户通过点击“QQ登录”图标按钮，或使用手机QQ扫码登入网站。

* nuget: https://www.nuget.org/packages/Myvas.AspNetCore.Authentication.QQConnect
* github: https://github.com/myvas/AspNetCore.Authentication.QQConnect

# Brothers and sisters
## AspNetCore.Authentication.WeixinOpen
An ASP.NET Core authentication middleware: WeixinOpen for https://open.weixin.qq.com (微信开放平台/微信扫码登录)


微信开放平台/微信扫码登录：须open.weixin.qq.com账号网站应用接入，用户使用微信扫码并确认后登入网站。

* nuget: https://www.nuget.org/packages/AspNetCore.Authentication.WeixinOpen
* github: https://github.com/myvas/AspNetCore.Authentication.WeixinOpen

## AspNetCore.Authentication.WeixinAuth
An ASP.NET Core authentication middleware: WeixinAuth for https://mp.weixin.qq.com （微信公众平台/微信网页授权登录）

微信公众平台/微信网页授权登录，须mp.weixin.qq.com账号，微信内置浏览器用户访问网站时自动登入网站。

* nuget: https://www.nuget.org/packages/AspNetCore.Authentication.WeixinAuth
* github: https://github.com/myvas/AspNetCore.Authentication.WeixinAuth

## Demo Online
* github: https://github.com/myvas/AspNetCore.Authentication.Demo
* demo: https://demo.auth.myvas.com

![alt https://demo.auth.myvas.com Weixin QrCode](http://mmbiz.qpic.cn/mmbiz_jpg/lPe5drS9euRQR1eCK5cGXaibHYL6vBR4pGLB34ju2hXCiaMQiayOU8w5GMfEH7WZsVNTnhLTpnzAC9xfdWuTT89OA/0)

## How to Use
### QQ互联平台
- 开发者注册: https://connect.qq.com
- 应用管理: https://connect.qq.com/manage.html

创建应用（网站应用，移动应用），并指定网站回调地址（例如：https://auth.myvas.com/qq )，记下AppId和AppKey

### ConfigureServices
```csharp
services.AddAuthentication()
    // 腾讯QQ登录：须connect.qq.com账号创建应用。用户通过点击“QQ登录”图标按钮，或使用手机QQ扫码登入网站。
    .AddQQConnect(options => 
    {
        options.AppId = Configuration["QQConnect:AppId"];
        options.AppKey = Configuration["QQConnect:AppKey"];

        options.CallbackPath = "/signin-qqconnect"; //默认

        QQConnectScopes.TryAdd(options.Scope,
            QQConnectScopes.get_user_info,
            QQConnectScopes.list_album, //需要额外开通权限，暂未实现
            QQConnectScopes.upload_pic, //需要额外开通权限，暂未实现
            QQConnectScopes.do_like); //需要额外开通权限，暂未实现
    };
```

### Configure
```csharp
    app.UseAuthentication();
```

### Dev
* .NET Core SDK 2.1.505



