# Introduction

The current way of hooking up with events triggered by documents is by creating an "Application event handler" and adding some methods to events on the "ContentService" service. However, this is not really usable if you have to execute code based on the document type alias. Indeed, the more document types you have, the more condition you'll need.

# Classes

"uDocumentEventHandler" allows the developer to create specific classes to handle events related to a specific document type.

For example, say we have the following document type: "documentTypeA", "documentTypeB", "documentTypeC".

To create a class handling events for all documents of type "Document Type A", you need to inherit from "DocumentEventHandler" and use a "DocumentEventHandler" attribute:

```
[DocumentEventHandler(Include = new[] { "documentTypeA" })]
public class DocumentTypeAEventHandler : DocumentEventHandler
{
    public override void Saving(IContentService contentService, SaveEventArgs<IContent> args)
    {
        base.Saving(contentService, args);
    }
}
```

This class ensures that the method "Saving" is executed as soon as a document type based on "Document Type A" is being saved. Most of the Umbraco events are supported.

You can also create classes that handle more document types than one by using the "Include" and "Exclude" properties of the attribute. If "Include" is not defined, the class handles all document type events except the ones defined in the "Exclude" property.

For example, the following classes handles all document type events except the ones triggered by "Document Type B":

```
[DocumentEventHandler(Exclude = new[] { "documentTypeB" })]
public class AllDocumentTypesButB : DocumentEventHandler
{
    public override void Saving(IContentService contentService, SaveEventArgs<IContent> args)
    {
        base.Saving(contentService, args);
    }
}
```

# Binding

There are two ways to enable "uDocumentEventHandler" but the important point is to specify the assembly that contains the document event handler classes. The first option is by using a "web.config appSettings" called "**AreaProg.uDocumentEventHandler.ModelAssembly**". For example:

```
<add key="AreaProg.uDocumentEventHandler.ModelAssembly" value="MyApp" />
```

Just by doing that, "uDocumentEventHandler" will register all classes that inehrit from "DocumentEventHandler" defined in that assembly. 

If you're relunctant to use an "appSettings", it is possible to register the classes with a custom application event handler:

```
public class DocumentEventHandlerRegistration : ApplicationEventHandler
{
    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        DocumentEventHandlerBinder.Bind(GetType().Assembly);
    }
}
```

All you need is to pass the assembly that contains the document event handler classes to the "Bind" method of the static class "DocumentEventHandlerBinder".
