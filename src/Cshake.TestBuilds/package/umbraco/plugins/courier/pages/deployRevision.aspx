<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/CourierPage.Master" CodeBehind="deployRevision.aspx.cs" Inherits="Umbraco.Courier.UI.Pages.deployRevision" %>
<%@ Register Namespace="umbraco.uicontrols" Assembly="controls" TagPrefix="umb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .cb{float: left; clear: left;}
         h2.propertypaneTitel{padding-bottom: 10px !Important; padding-top: 10px;}   
    </style>

    <script type="text/javascript">
       function displayStatusModal(title, message) {
           $('button').attr('disabled', 'disabled');
           var id = $('#statusId').val();

           if (message == undefined)
               message = "Please wait while Courier loads";


           UmbClientMgr.openModalWindow('plugins/courier/pages/status.aspx?statusId=' + id +"&message="+message, title, true, 500, 450);
       }

       $(document).ready(function () {

           AppendHelp("h2.propertypaneTitel", "/umbraco-pro/courier/courier-25-compare-and-deploy", "Watch video on how compare and deploy works");
            
           $('.openProvider').click(function () {
               $(this).closest('.revisionItemGroup').find('.revisionItems').show(100);
               $(this).closest('h3').find('.openProvider').hide();
               $(this).closest('h3').find('.allDependencies').show();
               $(this).closest('h3').find('.closeProvider').show();
           });

           $('.closeProvider').hide().click(function () {
               $(this).closest('.revisionItemGroup').find('.revisionItems').hide(100);
               $(this).closest('h3').find('.openProvider').show();
               $(this).closest('h3').find('.allDependencies').hide();
               $(this).closest('h3').find('.closeProvider').hide();
           });
       });
</script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
<umb:UmbracoPanel ID="panel" Text="Deploy revision" hasMenu="false" runat="server">


<umb:Pane ID="p_done" runat="server" Visible="false">
<div class="umbSuccess">
    <h4>Your changes has been installed on this instance</h4>
    <p>
        All items in the revision were created and updated without any issues
    </p>
</div>
</umb:Pane>


<umb:Pane ID="p_intro" runat="server">
    
    <input type="hidden" name="statusId" id="statusId" value="<%=Guid.NewGuid() %>" />
    <div style="float: right; margin-left: 30px; padding: 20px; border-left: #999 1px dotted; text-align: center;">
        <asp:button id="deployButton" runat="server" onclick="deploy" text="Install" /><br />
        <small>Install selected items</small>
    </div>
    
    <p>
        Below lists what items will be updated and created as new items. You can deselect the items you do
        not wish to transfer. However, if the complete transfer depends on an item that does not exist already, you 
        cannot de-select it.
    </p>

    <span class="options">
           <small>Overwrite items that already exist:<asp:CheckBox id="cb_overwriteExistingItems" Checked="true" runat="server" /></small>
           <small>Overwrite existing dependencies:<asp:CheckBox id="cb_overwriteExistingDependencies" Checked="true" runat="server" /></small>
           <small>Overwrite existing resources:<asp:CheckBox id="cb_overwriteExistingResources" Checked="false" runat="server" /></small>
    </span>

</umb:Pane>




<asp:panel runat="server" id="p_updates">
<h2 class="propertypaneTitel">Items which will be updated</h2>
       <asp:Repeater id="rp_providers_updates" runat="server" OnItemDataBound="bindProvider">
       <ItemTemplate>
         <div class="revisionItemGroup">
            <h3 style='background-image: url(<%# umbraco.IO.IOHelper.ResolveUrl(((Umbraco.Courier.Core.ItemProvider)Container.DataItem).ProviderIcon) %>);'><asp:Literal ID="lt_name" runat="server" />
               <img src="/umbraco/images/expand.png" class="openProvider" style="FLOAT: right"/>
               <img src="/umbraco/images/collapse.png" class="closeProvider" style="FLOAT: right"/>
            </h3>                        
            <ul class="revisionItems">
            <asp:Repeater ID="rp_changes" runat="server" OnItemDataBound="bindChanges">
            <ItemTemplate>
                <li class="revisionItem">
                <div class="cb" style="float:left;">
                    <asp:CheckBox ID="cb_transfer" Checked="true" runat="server" />
                </div>
                    <div>   
                       <asp:Literal ID="lt_item" runat="server" /><br />
                       <div class="dependencies"><asp:Literal ID="lt_desc" runat="server" /></div>
                       <asp:HiddenField ID="hf_key" runat="server" />
                    </div>
                </li>
            </ItemTemplate>
           </asp:Repeater>
           </ul>
          </div>
       </ItemTemplate>
       </asp:Repeater>
</asp:panel>

<asp:panel runat="server" id="p_new">
<h2 class="propertypaneTitel">Items which will be created</h2>
<asp:Repeater id="rp_providers_created" runat="server" OnItemDataBound="bindProvider">
       <ItemTemplate>
         <div class="revisionItemGroup">
            <h3 style='background-image: url(<%# umbraco.IO.IOHelper.ResolveUrl(((Umbraco.Courier.Core.ItemProvider)Container.DataItem).ProviderIcon) %>);'><asp:Literal ID="lt_name" runat="server" />
               <img src="/umbraco/images/expand.png" class="openProvider" style="FLOAT: right"/>
               <img src="/umbraco/images/collapse.png" class="closeProvider" style="FLOAT: right"/>
            </h3>
                        
            <ul class="revisionItems">
            <asp:Repeater ID="rp_changes" runat="server" OnItemDataBound="bindCreated">
            <ItemTemplate>
                <li class="revisionItem">
                <div class="cb" style="float:left;">
                    <asp:CheckBox ID="cb_transfer" Checked="true" runat="server" />
                </div>
                    <div>   
                       <asp:Literal ID="lt_item" runat="server" /><br />
                       <div class="dependencies"><asp:Literal ID="lt_desc" runat="server" /></div>
                       <asp:HiddenField ID="hf_key" runat="server" />
                    </div>
                </li>
            </ItemTemplate>
           </asp:Repeater>
           </ul>
          </div>
       </ItemTemplate>
    </asp:Repeater>
</asp:panel>

<asp:panel runat="server"  id="p_match">
<h2 class="propertypaneTitel">Items that already exist and did not change</h2>
  <asp:Repeater id="rp_providers_match" runat="server" OnItemDataBound="bindMatchProvider">
       
       <ItemTemplate>
         <div class="revisionItemGroup">
            <h3 style='background-image: url(<%# umbraco.IO.IOHelper.ResolveUrl(((Umbraco.Courier.Core.ItemProvider)Container.DataItem).ProviderIcon) %>);'><asp:Literal ID="lt_name" runat="server" />
               <img src="/umbraco/images/expand.png" class="openProvider" style="FLOAT: right"/>
               <img src="/umbraco/images/collapse.png" class="closeProvider" style="FLOAT: right"/>
            </h3>
                        
            <ul class="revisionItems">
            <asp:Repeater ID="rp_changes" runat="server" OnItemDataBound="bindMatches">
            <ItemTemplate>
                <li class="revisionItem">
                <div class="cb" style="float:left;">
                    <asp:CheckBox ID="cb_transfer" Checked="false" runat="server" />
                </div>
                    <div>   
                       <asp:Literal ID="lt_item" runat="server" /><br />
                       <asp:HiddenField ID="hf_key" runat="server" />
                    </div>
                </li>
            </ItemTemplate>
           </asp:Repeater>
           </ul>
          </div>
       </ItemTemplate>

       </asp:Repeater>
</asp:panel>


</umb:UmbracoPanel>
</asp:Content>