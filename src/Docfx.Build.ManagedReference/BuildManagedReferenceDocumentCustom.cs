// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Composition;

using Docfx.Build.Common;
using Docfx.DataContracts.Common;
using Docfx.DataContracts.ManagedReference;
using Docfx.Plugins;

namespace Docfx.Build.ManagedReference;

[Export(nameof(ManagedReferenceDocumentProcessor), typeof(IDocumentBuildStep))]
public class BuildManagedReferenceDocumentCustom : BaseDocumentBuildStep
{
    public override string Name => nameof(BuildManagedReferenceDocumentCustom);

    public override int BuildOrder => -1;

    public override void Build(FileModel model, IHostService host)
    {
        HandleXMLTag(model, host);
    }

    public override void Postbuild(ImmutableList<FileModel> models, IHostService host)
    {

    }

    private void HandleXMLTag(FileModel model, IHostService host)
    {
        var pageViewModel = (PageViewModel)model.Content;
        if(pageViewModel != null)
        {
            foreach(var item in pageViewModel.Items)
            {
                if (pageViewModel.Metadata.TryGetValue("name", out var name))
                {
                    pageViewModel.Metadata.Add("name", name);
                }
                // if(item.Summary != null)
                // {
                    item.Name += "CUSTOM";
                    item.NameWithType += "CUSTOM";
                    //item.Examples.Add("CUSTOM");
                    //item.Names;
                    // }
            }
        }


    }
}
