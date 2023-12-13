/// <amd-dependency path="dojo/_base/declare" name="dojo_declare" />
/// <amd-dependency path="dijit/_Widget" name="_Widget" />
/// <amd-dependency path="dijit/_TemplatedMixin" name="_TemplatedMixin" />
/// <amd-dependency path="dijit/_WidgetsInTemplateMixin" name="_WidgetsInTemplateMixin" />
/// <amd-dependency path="dojo/text!/StaticView/LoginWidget.html" name="template" />


declare const dojo_declare;
declare const _Widget;
declare const _TemplatedMixin;
declare const _WidgetsInTemplateMixin
declare const template


class LoginWidget {
    own: (value: any) => void;
    set: (key: string, value: any) => void;
    _set: (key: string, value: any) => void;
    inherited: (value: any) => void;

    private templateString: any = template;

    constructor() {
        console.log('constructor');
    }

    postCreate() {
        this.inherited(arguments);
        console.log('postCreate');
        console.log('postCreate1');
    }




}
export = dojo_declare("LoginWidget", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin], LoginWidget.prototype);


