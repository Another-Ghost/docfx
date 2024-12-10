# 介绍
本项目使用 [DocFX](https://dotnet.github.io/docfx/index.html) 来自动生成项目代码 API 文档。

DocFX 是一个开源的文档生成工具，主要用于从源代码和 Markdown 文件生成静态文档（支持HTML，PDF 等格式输出），
尤其是对于 .NET 项目，DocFX 能够从 C# 源代码注释中生成 API 文档描述。

项目会在每次代码提交后自动触发 DocFX 的构建任务，生成最新的 API 文档，不需要手动触发。
# C# 代码注释规范
C# 文档注释使用 XML 元素来定义输出文档的结构。
XML 注释通常以三斜杠 `///` 开头，后面跟随注释内容：

```csharp
/// <summary>
/// 这是一个方法的简单描述
/// </summary>
public void MyMethod()
{
    // 方法的实现
}
```
通过在 C# 代码中正确使用 XML 注释，DocFX 可以自动提取相关信息用于文档生成。XML 标记内还支持 Markdown 语法标记。
> XML 注释并不会影响 API 文档中的代码结构，只是提供辅助说明信息。

## 常用的 XML 注释标记
以下是一些常用的 XML 注释标记（tag）规范，更多内容参考 [Microsoft 文档](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/xmldoc/recommended-tags)。

以下是常用的 XML 注释标记和它们的作用：

### `<summary>`
用于简要描述类、方法、属性、字段等的功能。
所有 public 方法都应该有 `<summary>` 标记。

```csharp
/// <summary>
/// 计算两个整数的和
/// </summary>
public int Add(int a, int b)
{
    return a + b;
}
```

### `<remarks>`
用于提供更多的补充信息或备注。

```csharp
/// <summary>
/// 计算两个整数的和
/// </summary>
/// <remarks>
/// 此方法实现了简单的加法操作，适用于整数类型
/// </remarks>
public int Add(int a, int b)
{
    return a + b;
}
```
### `<param>`
用于描述方法的参数。每个参数都应有一个 `<param>` 标记，包含 `name` 属性指明参数名。

```csharp
/// <summary>
/// 计算两个整数的和
/// </summary>
/// <param name="a">第一个整数</param>
/// <param name="b">第二个整数</param>
public int Add(int a, int b)
{
    return a + b;
}
```

### `<returns>`
描述方法的返回值。

```csharp
/// <summary>
/// 获取字符串的长度
/// </summary>
/// <returns>返回字符串的长度</returns>
public int GetLength(string str)
{
    return str.Length;
}
```
### `<exception>`
描述方法可能抛出的异常。

```csharp
/// <summary>
/// 从文件中读取内容
/// </summary>
/// <param name="filePath">文件路径</param>
/// <returns>文件内容</returns>
/// <exception cref="FileNotFoundException">文件未找到时抛出</exception>
/// <exception cref="UnauthorizedAccessException">没有权限时抛出</exception>
public string ReadFile(string filePath)
{
    // 读取文件的实现
}
```

### `<typeparam>`
用于描述泛型类或方法的类型参数。

```csharp
/// <summary>
/// 泛型方法示例
/// </summary>
/// <typeparam name="T">泛型类型参数</typeparam>
/// <param name="value">传入的值</param>
/// <returns>返回相同类型的值</returns>
public T Echo<T>(T value)
{
    return value;
}
```

### `<example>`
提供代码示例，帮助用户理解如何使用方法或类。

```csharp
/// <summary>
/// 加法示例
/// </summary>
/// <example>
/// <code>
/// var result = Add(2, 3); // result == 5
/// </code>
/// </example>
public int Add(int a, int b)
{
    return a + b;
}
```
## 项目自定义 XML 注释标记
项目中定义了一些自定义的 XML 注释标记，用于生成和控制特定的文档内容。
目前这些自定义标记都**需要在 `<summary>` 标记内使用**，自定义标记内的内容并不会在生成的文档中 `summary` 所对应的部分中显示，而是根据各自设定的功能来修改文档内容。
### <alias>
用于为类、方法、属性等定义别名，别名会加在对应元素名称前显示。
如：
```csharp
/// <summary>
/// <alias>
/// 销毁实体
/// </alias>
/// </summary>
public class DestroyEntityNode: ServerFlowNode
{
    ...
}
```
文档实际显示效果为：
![AliasTagExample](/resources/AliasTagExample.png)
`Alias`标记通常用于为 API 元素添加中文名称，以便于非程序部分查找和搜索对应内容。
如技能编辑器中，一些技能节点编辑器下只对外显示中文名称，这时候就需要为节点添加中文名称`Alias`标记来方便策划查找对应节点。 
## Markdown 内容支持
DocFX 支持 Markdown 语法识别，可以使用 Markdown 标记来格式化文本。
Markdown 内容需在标记内部使用，如 `<summary>`、`<remarks>` 等。
如：
```csharp
    /// <summary>
    /// 这是一个示例方法，它展示了如何在 XML 注释中插入 Markdown 内容。
    /// ## 标题
    /// 你可以使用各种 Markdown 语法，例如：
    /// 
    /// - **加粗文本**
    /// - *斜体文本*
    /// - `代码`
    /// - [链接](https://example.com)
    /// 
    /// 甚至可以插入代码块：
    /// ```csharp
    /// public void ExampleMethod() {
    ///     Console.WriteLine("Hello, World!");
    /// }
    /// ```
    /// </summary>
    /// <remarks>
    /// 图片等资源需放在 `Client/Projects/Documentation/Resources` 路径下。
    /// - 示例：
    /// ![APIExample](/resources/APIExample.png)
    /// </remarks>
    public class DestroyEntityNode: ServerFlowNode
    {
        ...
    }
```
渲染后的文档效果如下：
![MarkdownExample](/resources/MarkdownExample.png)
### 注意事项
- 图片等资源需放在 `Client/Projects/Documentation/Resources` 路径下。
## 示例
```csharp
namespace CombatModules.ServerLogic
{
    /// <summary>
    /// <alias>
    /// 销毁实体
    /// </alias>
    /// **销毁** <see cref="entityId"/> 对应的实体，
    /// 详见[EasyECS](https://gitlab.in.ys4fun.com/m03/plugins/easyecs)。
    /// </summary>
    /// <remarks>
    /// 只需调用一次。
    /// </remarks>
    [Name("销毁实体")]
    [Category("技能逻辑/行为")]
    [Description("销毁某一个实体")]
    public class DestroyEntityNode: ServerFlowNode
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        /// <remarks>
        /// 要销毁的 Entity 的 ID 
        /// </remarks>
        public ValueInput<int> entityId;
        protected override void RegisterPorts()
        {
            base.RegisterPorts();
            entityId = AddValueInput<int>("实体ID");
        }

        protected override void Trigger()
        {
            base.Trigger();
            _serverLookup.InputOpDestroyEntityEvent(0, entityId.value);
        }
    }
}
```
生成文档效果如下：
![FullExample](/resources/FullExample.png)
# 文档生成相关代码规范
## Namespace
由于文档的页面的层级结构是根据命名空间来定义的，所以请确保命名空间的层级结构清晰。
如想要把同一类内容显示在同一个父页面中，他们就需要具有相同的 Namespace。
