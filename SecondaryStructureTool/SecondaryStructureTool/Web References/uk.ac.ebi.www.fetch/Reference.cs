﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34014.
// 
#pragma warning disable 1591

namespace SecondaryStructureTool.uk.ac.ebi.www.fetch {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSDbfetchSoapBinding", Namespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
    public partial class WSDBFetchServerService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getDbFormatsOperationCompleted;
        
        private System.Threading.SendOrPostCallback fetchDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback fetchBatchOperationCompleted;
        
        private System.Threading.SendOrPostCallback getSupportedDBsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getSupportedFormatsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getFormatStylesOperationCompleted;
        
        private System.Threading.SendOrPostCallback getSupportedStylesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSDBFetchServerService() {
            this.Url = global::SecondaryStructureTool.Properties.Settings.Default.SecondaryStructureTool_uk_ac_ebi_www_fetch_WSDBFetchServerService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event getDbFormatsCompletedEventHandler getDbFormatsCompleted;
        
        /// <remarks/>
        public event fetchDataCompletedEventHandler fetchDataCompleted;
        
        /// <remarks/>
        public event fetchBatchCompletedEventHandler fetchBatchCompleted;
        
        /// <remarks/>
        public event getSupportedDBsCompletedEventHandler getSupportedDBsCompleted;
        
        /// <remarks/>
        public event getSupportedFormatsCompletedEventHandler getSupportedFormatsCompleted;
        
        /// <remarks/>
        public event getFormatStylesCompletedEventHandler getFormatStylesCompleted;
        
        /// <remarks/>
        public event getSupportedStylesCompletedEventHandler getSupportedStylesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("getDbFormatsReturn")]
        public string[] getDbFormats(string db) {
            object[] results = this.Invoke("getDbFormats", new object[] {
                        db});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getDbFormatsAsync(string db) {
            this.getDbFormatsAsync(db, null);
        }
        
        /// <remarks/>
        public void getDbFormatsAsync(string db, object userState) {
            if ((this.getDbFormatsOperationCompleted == null)) {
                this.getDbFormatsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDbFormatsOperationCompleted);
            }
            this.InvokeAsync("getDbFormats", new object[] {
                        db}, this.getDbFormatsOperationCompleted, userState);
        }
        
        private void OngetDbFormatsOperationCompleted(object arg) {
            if ((this.getDbFormatsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDbFormatsCompleted(this, new getDbFormatsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("fetchDataReturn")]
        public string fetchData(string query, string format, string style) {
            object[] results = this.Invoke("fetchData", new object[] {
                        query,
                        format,
                        style});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void fetchDataAsync(string query, string format, string style) {
            this.fetchDataAsync(query, format, style, null);
        }
        
        /// <remarks/>
        public void fetchDataAsync(string query, string format, string style, object userState) {
            if ((this.fetchDataOperationCompleted == null)) {
                this.fetchDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfetchDataOperationCompleted);
            }
            this.InvokeAsync("fetchData", new object[] {
                        query,
                        format,
                        style}, this.fetchDataOperationCompleted, userState);
        }
        
        private void OnfetchDataOperationCompleted(object arg) {
            if ((this.fetchDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fetchDataCompleted(this, new fetchDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("fetchBatchReturn")]
        public string fetchBatch(string db, string ids, string format, string style) {
            object[] results = this.Invoke("fetchBatch", new object[] {
                        db,
                        ids,
                        format,
                        style});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void fetchBatchAsync(string db, string ids, string format, string style) {
            this.fetchBatchAsync(db, ids, format, style, null);
        }
        
        /// <remarks/>
        public void fetchBatchAsync(string db, string ids, string format, string style, object userState) {
            if ((this.fetchBatchOperationCompleted == null)) {
                this.fetchBatchOperationCompleted = new System.Threading.SendOrPostCallback(this.OnfetchBatchOperationCompleted);
            }
            this.InvokeAsync("fetchBatch", new object[] {
                        db,
                        ids,
                        format,
                        style}, this.fetchBatchOperationCompleted, userState);
        }
        
        private void OnfetchBatchOperationCompleted(object arg) {
            if ((this.fetchBatchCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.fetchBatchCompleted(this, new fetchBatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("getSupportedDBsReturn")]
        public string[] getSupportedDBs() {
            object[] results = this.Invoke("getSupportedDBs", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getSupportedDBsAsync() {
            this.getSupportedDBsAsync(null);
        }
        
        /// <remarks/>
        public void getSupportedDBsAsync(object userState) {
            if ((this.getSupportedDBsOperationCompleted == null)) {
                this.getSupportedDBsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSupportedDBsOperationCompleted);
            }
            this.InvokeAsync("getSupportedDBs", new object[0], this.getSupportedDBsOperationCompleted, userState);
        }
        
        private void OngetSupportedDBsOperationCompleted(object arg) {
            if ((this.getSupportedDBsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getSupportedDBsCompleted(this, new getSupportedDBsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("getSupportedFormatsReturn")]
        public string[] getSupportedFormats() {
            object[] results = this.Invoke("getSupportedFormats", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getSupportedFormatsAsync() {
            this.getSupportedFormatsAsync(null);
        }
        
        /// <remarks/>
        public void getSupportedFormatsAsync(object userState) {
            if ((this.getSupportedFormatsOperationCompleted == null)) {
                this.getSupportedFormatsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSupportedFormatsOperationCompleted);
            }
            this.InvokeAsync("getSupportedFormats", new object[0], this.getSupportedFormatsOperationCompleted, userState);
        }
        
        private void OngetSupportedFormatsOperationCompleted(object arg) {
            if ((this.getSupportedFormatsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getSupportedFormatsCompleted(this, new getSupportedFormatsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("getFormatStylesReturn")]
        public string[] getFormatStyles(string db, string format) {
            object[] results = this.Invoke("getFormatStyles", new object[] {
                        db,
                        format});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getFormatStylesAsync(string db, string format) {
            this.getFormatStylesAsync(db, format, null);
        }
        
        /// <remarks/>
        public void getFormatStylesAsync(string db, string format, object userState) {
            if ((this.getFormatStylesOperationCompleted == null)) {
                this.getFormatStylesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetFormatStylesOperationCompleted);
            }
            this.InvokeAsync("getFormatStyles", new object[] {
                        db,
                        format}, this.getFormatStylesOperationCompleted, userState);
        }
        
        private void OngetFormatStylesOperationCompleted(object arg) {
            if ((this.getFormatStylesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getFormatStylesCompleted(this, new getFormatStylesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch", ResponseNamespace="http://www.ebi.ac.uk/ws/services/WSDbfetch")]
        [return: System.Xml.Serialization.SoapElementAttribute("getSupportedStylesReturn")]
        public string[] getSupportedStyles() {
            object[] results = this.Invoke("getSupportedStyles", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getSupportedStylesAsync() {
            this.getSupportedStylesAsync(null);
        }
        
        /// <remarks/>
        public void getSupportedStylesAsync(object userState) {
            if ((this.getSupportedStylesOperationCompleted == null)) {
                this.getSupportedStylesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSupportedStylesOperationCompleted);
            }
            this.InvokeAsync("getSupportedStyles", new object[0], this.getSupportedStylesOperationCompleted, userState);
        }
        
        private void OngetSupportedStylesOperationCompleted(object arg) {
            if ((this.getSupportedStylesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getSupportedStylesCompleted(this, new getSupportedStylesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void getDbFormatsCompletedEventHandler(object sender, getDbFormatsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDbFormatsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDbFormatsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void fetchDataCompletedEventHandler(object sender, fetchDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fetchDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal fetchDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void fetchBatchCompletedEventHandler(object sender, fetchBatchCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class fetchBatchCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal fetchBatchCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void getSupportedDBsCompletedEventHandler(object sender, getSupportedDBsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getSupportedDBsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getSupportedDBsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void getSupportedFormatsCompletedEventHandler(object sender, getSupportedFormatsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getSupportedFormatsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getSupportedFormatsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void getFormatStylesCompletedEventHandler(object sender, getFormatStylesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getFormatStylesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getFormatStylesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void getSupportedStylesCompletedEventHandler(object sender, getSupportedStylesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getSupportedStylesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getSupportedStylesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591