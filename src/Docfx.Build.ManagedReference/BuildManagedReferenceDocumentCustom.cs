// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Composition;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Linq;
using Docfx.Build.Common;
using Docfx.DataContracts.Common;
using Docfx.DataContracts.ManagedReference;
using Docfx.Plugins;
using YamlDotNet.Core.Tokens;

namespace Docfx.Build.ManagedReference;

[Export(nameof(ManagedReferenceDocumentProcessor), typeof(IDocumentBuildStep))]
public class BuildManagedReferenceDocumentCustom : BaseDocumentBuildStep
{
    public override string Name => nameof(BuildManagedReferenceDocumentCustom);

    public override int BuildOrder => -1;

    public override void Build(FileModel model, IHostService host)
    {
        var pageViewModel = (PageViewModel)model.Content;
        if(pageViewModel != null)
        {
            foreach(var item in pageViewModel.Items)
            {
                if(item.Summary != null)
                {
                    string customName, newSummary;
                    HandleXMLTag(item.Summary, AliasTag, out customName, out newSummary);
                    if (customName != null)
                    {
                        item.Name = customName + ' ' + item.Name;
                        item.Summary = newSummary;

                    }
                }
            }
        }
    }

    public override void Postbuild(ImmutableList<FileModel> models, IHostService host)
    {

    }

    private void HandleXMLTag(string input, string tag, out string tagContent, out string newInput)
    {
        tagContent = null;
        newInput = input;
        // Find and consume the XML tag in the input
        var startTag = $"<{tag}>";
        var endTag = $"</{tag}>";
        var startIndex = input.IndexOf(startTag);
        var endIndex = input.IndexOf(endTag);

        if (startIndex != -1 && endIndex != -1)
        {
            tagContent = input.Substring(startIndex + startTag.Length, endIndex - (startIndex + startTag.Length));
            if(tagContent != null)
            {
                // Remove the tag from the input
                newInput = input.Remove(startIndex, endIndex + endTag.Length - startIndex);
                // Remove the whitespace from the tag content
                tagContent = tagContent.Trim();

            }
        }
    }

    private static string AliasTag => "alias";
}
