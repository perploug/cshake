<%@ Page MasterPageFile="../MasterPages/CourierDialog.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="CommitItem.aspx.cs" Inherits="Umbraco.Courier.UI.Dialogs.CommitItem" %>
<%@ Register Src="../usercontrols/SystemItemSelector.ascx" TagName="SystemItemSelector"
    TagPrefix="courier" %>
<%@ Register Src="../usercontrols/DependencySelector.ascx" TagName="DependencySelector"
    TagPrefix="courier" %>
<%@ Register Namespace="umbraco.uicontrols" Assembly="controls" TagPrefix="umb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server" Visible="false" >
    <script type="text/javascript" src="../scripts/RevisionJsonRender.js"></script>
    <style type="text/css">
        .groupWarning h3{color: Red;}
    </style>    

    <script type="text/javascript">
        
        var tb_ChildItemIDs
        var ul_selectTedList


        jQuery(document).ready(function () {
            AppendHelp("h2.propertypaneTitel", "/umbraco-pro/courier/courier-25-right-click-deployment/", "Watch video on how to deploy");
             jQuery("#buttons input").click(function () {
               jQuery("#buttons").hide();
                    jQuery(".bar").show();
                    var msg = jQuery("#loadingMsg").html();
                    if (msg != '')
                        jQuery(".bar").find("small").text(msg);
                });
        });

        function SetSelectedChildren() {
            var ids = "";
            jQuery(".itemchb::checked").each(function () {
                ids += jQuery(this).attr("id") + ";";
            });
            selectedChildItems.val(ids);
        }


        function updateResources(s_root, parentDom) {
            jQuery.ajax({
                type: "POST",
                url: "CommitItem.aspx/GetFilesAndFolders",
                data: "{'root':'" + s_root + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (meh) {

                    parentDom.children().remove();

                    var list = "<ul>";

                    jQuery(meh.d).each(function (index, domEle) {
                        var html = "";
                        if (domEle.Type == "file")
                            html = "<li class='file'><a href='#' onClick='addFile(\"" + domEle.Path + "\",\"" + domEle.Name + "\"); return false;'>" + domEle.Name + "</a></li>";
                        else
                            html = "<li class='folder'><a href='#'onClick='addFolder(\"" + domEle.Path + "\",\"" + domEle.Name + "\"); return false;'>" + domEle.Name + "</a><span><img class='expand' onclick='updateResources(\"" + domEle.Path + "\", jQuery(this.parentNode)); return false;' src='/umbraco/images/nada.gif'/></span></li>";

                        list += html;
                    });

                    list += "</ul>";

                    parentDom.append(list);
                }
            });
        }

        function addFile(path, name) {

            var currentList = tb_ChildItemIDs.val();

            if (currentList.indexOf("|" + path) < 0) {

                var html = "<li class='file'><a href='#' onClick='remove(this,\"" + path + "\"); return false;'>" + name + "</a></li>";
                ul_selectTedList.append(html);
                tb_ChildItemIDs.val(tb_ChildItemIDs.val() + "|" + path);
            }
        }

        function addFolder(path, name) {

            var currentList = tb_ChildItemIDs.val();

            if (currentList.indexOf("|" + path) < 0) {
                var html = "<li class='folder'><a href='#' onClick='remove(this,\"" + path + "\"); return false;'>" + name + "</a></li>";
                ul_selectTedList.append(html);
                tb_ChildItemIDs.val(tb_ChildItemIDs.val() + "|" + path);
            }
        }

        function remove(elm, path) {
            var list = tb_ChildItemIDs.val().replace(path, "").replace("||", "|");
            tb_ChildItemIDs.val(list);
            jQuery(elm).parent().remove();
        }
    </script>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <umb:Pane runat="server" ID="paneNoRepositoriesAvailable" Text="No Locations available"
        Visible="false">
        <p>
            Please set a valid reposity (target for the transfer), without repositories you
            can't transfer an item</p>
    </umb:Pane>






    <umb:Pane runat="server" ID="paneSelectItems" Text="Deploy items" Visible="false">
        <script type"text/javascript">
            var cb_all;
            var dest_ddl;
            var selectedChildItems;

            function processCheckedNodes(nodes) {
                selectedChildItems = jQuery(".systemItemSelectorTextBox");

                var nodesAsString = "";

                jQuery.each(nodes, function (index, node) {
                    
                    var key = node.data.key;
                    if (node.hasChildren() == undefined) {
                        key = "$" + key;
                    }

                    nodesAsString += key + ";"
                });

                selectedChildItems.val(nodesAsString);
            }

            jQuery(document).ready(function () {
                cb_all = jQuery(".cbTransferAllChildren");
                dest_ddl = jQuery(".destinationDDL");
                cb_all.change(function () {
                    if (!cb_all.attr("checked"))
                        jQuery("#specificChildSelection").show();
                    else
                        jQuery("#specificChildSelection").hide();

                    jQuery(this).blur();
                });

                jQuery(".itemchb").change(function () {
                    SetSelectedChildren();
                });

                if (dest_ddl.length > 0) {
                    jQuery(".submitButton").attr('disabled', 'disabled');
                    jQuery(".destinationDDL").change(function () {
                        var ddl = jQuery(this);
                        if (ddl.val() != '')
                            jQuery(".submitButton").removeAttr('disabled');
                        else
                            jQuery(".submitButton").attr('disabled', 'disabled');
                    });
                }       
            });

            </script>
        <div>
           
        <asp:placeholder runat="server" id="phSelectDestination" visible="false">
            <p>Target: <asp:dropdownlist runat="server" id="stepOneDDl" cssclass="destinationDDL" /></p>
        </asp:placeholder>
            
        <asp:placeholder runat="server" id="phdefaultDestination" visible="false">
            <p>Target: <strong><asp:literal id="ltDestination" runat="server" /></strong></p>
        </asp:placeholder>
        
         <asp:placeholder runat="server" id="phdefaultLimit" visible="true">
         <p>    
            Transfer: 
            <asp:dropdownlist runat="server" id="ddlDepth">
                <asp:ListItem Value="0" selected="true">Selected items and all dependencies</asp:ListItem>
                <asp:ListItem Value="1">Selected items only</asp:ListItem>
                <asp:ListItem Value="2">Selected + 1 Dependency level</asp:ListItem>
                <asp:ListItem Value="3">Selected + 2 Dependency levels</asp:ListItem>
                <asp:ListItem Value="4">Selected + 3 Dependency levels</asp:ListItem>
            </asp:dropdownlist> 
         </p>
         </asp:placeholder>

        </div>

        <asp:placeholder runat="server" id="phChildSelection" visible="false">
        <div style="border-top: 1px solid #ccc">
            <p>
               The item you have selected, has child items available, do you want to deploy all of them or select specific items to deploy?
            </p>
           
            <!-- Only needed when the item has children -->
             <asp:checkbox runat="server" ID="cbTransferAllChildren" cssclass="cbTransferAllChildren" Text="Yes, transfer all children as well" Checked="true"></asp:checkbox>
             
                 <div id="specificChildSelection" style="display: none">
                    <div id="itemList">
                        <courier:SystemItemSelector ID="SystemItemSelector" runat="server" />
                    </div>
                 </div>

                 <div style="display:none;">
                    <asp:textbox runat="server" ID="txtChildItemIDs" cssclass="systemItemSelectorTextBox" textmode="multiline"></asp:textbox>
                </div>

                <div id="loadingMsg">Collecting items...</div>
          </div>
        </asp:placeholder>
    </umb:Pane>
    
    <asp:placeholder runat="server" id="stepOneButtons" visible="false">
    <br />
    <div id="buttons">
        <asp:button runat="server" text="Deploy" cssclass="submitButton" onclick="oneSteptransfer"/>
        <em><%= umbraco.ui.Text("or") %></em> 
        <asp:linkbutton onclick="transfer" runat="server" text="Go to advanced settings" />
    </div>
    
    <div class="bar" style="display: none" align="center">
        <img src="/umbraco_client/images/progressbar.gif" alt="loading" />
        <small></small>
    </div>
    </asp:placeholder>
    

    <umb:Pane runat="server" ID="paneSelectDefaults" Visible="false" Text="Should anything be overwritten?">
        <p>
            The item you have selected has dependencies, should courier override existing items
            on the target or only deploy those that doesn't exist already.
        </p>
        <br />
        <p>
            <asp:checkbox runat="server" id="cbOverWriteItems" text="Overwrite existing items"
                checked="true"></asp:checkbox>
        </p>
        <p>
            <asp:checkbox runat="server" id="cbOverWriteDeps" text="Overwrite existing dependencies"
                checked="true"></asp:checkbox>
        </p>
        <p>
            <asp:checkbox runat="server" id="cbOverWriteFiles" text="Overwrite existing files"
                checked="true"></asp:checkbox>
        </p>

       
        <div id="loadingMsg">
            Determining dependencies and resources...</div>
    </umb:Pane>
   
   
    <umb:Pane runat="server" ID="paneSelectResources" Visible="false" Text="Add additional files?">
        <script type="text/javascript">
            jQuery(document).ready(function () {
                tb_ChildItemIDs = jQuery("input.tbResources");
                ul_selectTedList = jQuery("#selectList ul");


                jQuery(".resourcePicker").click(function () {
                    jQuery("#resourceListWrapper").show();
                    updateResources("~/", jQuery("#resourceList"));
                    return false;
                });

            });
        </script>

        <p>
            If you know of any additional files required by this change, please add them below
        </p>
        <a href="#" class="resourcePicker">Add files or folders to deployment</a>
        <div style="position: relative; height: 270px; display: none; border: 1px solid #efefef;"
            id="resourceListWrapper">
            <div id="resourceList" style="position: absolute; top: 0px; left: 0px; border-left: #efefef 1px solid;
                height: 250px; width: 290px; padding: 5px; padding-right: 0px; overflow: auto;">
            </div>
            <div id="selectList" style="position: absolute; top: 0px; right: 0px; height: 250px;
                width: 250px; padding: 5px; overflow: auto;">
                <ul>
                    <li style="display: none;">.</li>
                </ul>
            </div>
        </div>
        <div id="loadingMsg">
            Adding additional resources...</div>
    </umb:Pane>
    <div style="display: none">
        <asp:textbox id="tb_Resources" class="tbResources" runat="server" />
    </div>   
    
    
    <umb:Pane runat="server" ID="paneSelectDestination" Visible="false" Text="Select destination">
        <script type="text/javascript">
            jQuery(document).ready(function () {

                var ddl = jQuery(".destinationDDL");

                if(ddl.val() == "")
                    jQuery(".submitButton").attr('disabled', 'disabled');

                ddl.change(function () {
                    var ddl = jQuery(this);
                    if (ddl.val() != '')
                        jQuery(".submitButton").removeAttr('disabled');
                    else
                        jQuery(".submitButton").attr('disabled', 'disabled');
                });
            });         
        </script>
        <p>
            Where should the items be transfered to?</p>
        <p>
            <asp:dropdownlist runat="server" id="ddDestination" cssclass="destinationDDL"></asp:dropdownlist>
        </p>
        <div id="loadingMsg">
            building summary...</div>
    </umb:Pane>
    
    

    <asp:Panel runat="server" ID="paneConfirm" Visible="false">
    

    <script type="text/javascript">

        var g = '<asp:literal id="js_directory" runat="server" />';
        var target = '<asp:literal id="js_target" runat="server" />';
        var interval;

        jQuery(document).ready(function () {
            jQuery("#buttons").hide();
            interval = setInterval("update()", 1000);
        });

        function update() {
            updateCurrent();
        }        
    </script>

    <div id="loader">
     <h2 class="propertypaneTitel">Packaging your items</h2>
     <ul class='taskmanager'>
        <li>Please wait...</li>
     </ul>
    </div>


    <div id="revisionTabview" style="display: none">
    <umb:TabView ID="tv_revisionTabs" Width="580" Height="320" runat="server" AutoResize="false" />

    <asp:panel id="ph_revision" runat="server"><div style="margin-top: 7px"  id="revision"></div></asp:panel>

    <asp:panel id="ph_resources" runat="server"><div style="margin-top: 7px" id="resources"></div></asp:panel>

    <br style="clear: both" />
    </div>

    <span style="display: none">
        <asp:textbox id="tb_uncheckedItems" cssclass="tb_uncheckedItems" runat="server" textmode="multiline" />
        <asp:textbox id="tb_uncheckedResources" cssclass="tb_uncheckedResources" runat="server" textmode="multiline" />    
    </span>

    <div id="loadingMsg">Adding extraction to the task manager...</div>
    </asp:Panel>
    
    <br />
    <asp:placeholder runat="server" id="phButtons">

    <div id="buttons">
        <asp:button runat="server" text="Continue" cssclass="submitButton" onclick="transfer"/>
        <em><%= umbraco.ui.Text("or") %></em> <a href="#" onclick="UmbClientMgr.closeModalWindow(); return false;"><%= umbraco.ui.Text("cancel") %></a>
    </div>

    
    <div class="bar" style="display: none" align="center">
        <img src="/umbraco_client/images/progressbar.gif" alt="loading" />
        <small></small>
    </div>

    </asp:placeholder>

    
    
</asp:Content>
