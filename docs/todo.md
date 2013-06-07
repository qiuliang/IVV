# IVV网站开发

> 已经完成了一部分，在原有基础上继续开发

## TODO

### 准备

- 安装vs2012
- 按代码风格要求设置下环境
- 查看sqlite数据实体文件
- 把网站run起来

### 需求

- 菜单显示改变为直接显示，不需要第一个“菜单”点击再显示下列了
- 主菜单中英文一起显示
- “产品保养”“画册”和“商店”3个栏目取消

#### 首页底部版权公司信息，实现在后台

首页为静态html文件，底部的加载采取ajax方式获取。

1. 修改实体SiteInfo和对应的表结构
	
		新增属性Copyright string (varchar(4000) null)

###### 在vs2012里面没有sqlite的数据源，无法通过数据库来更新模型

从网上下载的`SQLite-1.0.66.0-setup.exe`可以为VS2010安全数据源适配。
目前的解决方案如下：

	1，先修改sqlite数据库的表结构。
	2，通过vs2010来建立一个同名的SiteEntity，并更新模型。
	3，使用xml编辑器打开SiteModel.edmx文件，复制其中的SSDL、CSDL、C-S Mapping节点内容到vs2012的edmx文件。
	4，同时修改vs2012版edmx文件SSDL Schema的命名空间如下：xmlns="http://schemas.microsoft.com/ado/2009/11/edm/ssdl"
	5，同时修改vs2012版edmx文件C-S Mapping节点下的Mapping命名空间如下：http://schemas.microsoft.com/ado/2009/11/mapping/cs
