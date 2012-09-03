<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/CourierPage.Master" CodeBehind="ViewTaskManager.aspx.cs" Inherits="Umbraco.Courier.UI.Pages.ViewTaskManager" %>
<%@ Import Namespace="Umbraco.Courier.UI" %>
<%@ Register Namespace="umbraco.uicontrols" Assembly="controls" TagPrefix="umb" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

    var detailsPage = '<%= Umbraco.Courier.UI.UIConfiguration.RevisionViewPage %>';
    var redirect = '<%= Request.QueryString["redirWhenDone"] %>';
    var revision = '<%= Request.QueryString["revision"] %>';
    
    var isDialog = '<%= Request.QueryString["dialog"]%>';
    var taskId = '<%= Request.QueryString["taskId"]%>';

    

     jQuery(document).ready(function () {
        setInterval("updateCurrent()", 1000);
        setInterval("updateView()", 1000);
     });
     
     function updateCurrent() {

         var parentDom = jQuery("#current");
         var parentList = parentDom.find("ul");
         var show = false;

         jQuery.ajax({
             type: "POST",
             url: "ViewTaskManager.aspx/GetCurrentTask",
             contentType: "application/json; charset=utf-8",
             dataType: "json",


             success: function (meh) {
                 parentList.children().remove();

                 var list = "";
                

                 jQuery(meh.d).each(function (index, el) {
  

                     if (el.TaskType != null) {
                         var html = "<li class='task " + el.TaskType + "'>";
                         var date = new Date(parseInt(el.TimeStamp.substr(6)));
                         var name = el.RevisionAlias;

                         html += "<div class='label'><h3><a href='viewRevisionDetails.aspx?revision=" + el.RevisionAlias + "'>" + el.TaskType + "</a></h3><small>" + el.FriendlyName + "</small></div>";
                         html += "<div class='progress'><div class='bar'><span style='width: " + el.Progress + "%;'></span></div><small>" + el.State + "</small></div>";

                         if (el.Message != null)
                             html += "<div class='message'>" + el.Message + "</div>";

                         html += "<br style='clear: both'/></li>";
                         list += html;

                         show = true;
                     }
                 });

                 parentList.append(list);

                 if (show)
                     parentDom.show();
                 else {
                     parentDom.hide();
                 }
             }
         });
              
     }

     function updateView() {
        updateTaskList("GetQueue", jQuery("#queue"));
        updateTaskList("GetProcessed", jQuery("#processed"));
    }

    function showDoneMessage(el) {


        if (isDialog == 'true') {
            var fb = jQuery("#feedback");

            //  if (!fb.is(':visible')) {
            var html = "<p>";

            fb.attr("class", "");

            if (el.State == "completed") {
                fb.addClass("success");
                html += "<h4>'" + el.FriendlyName + "' was completed successfully</h4>";
            } else {
                fb.addClass("error");
                html += "<h4>'" + el.FriendlyName + "'  was not completed, as it returned an error</h4>";
            }

            html += "<p><a href='#' onclick='UmbClientMgr.closeModalWindow(); return false;'>Close this window</a></p>";

            fb.html(html);
            fb.show();
            //      }
        } else {

            if (redirect == 'true' && el.State == "completed") {
                self.location = detailsPage + "?revision=" + revision;
            }            
        }


    }

    function sortDate(a, b) {
        return a.Ticks > a.Ticks ? 1 : -1;
    }


    var latest = '';

    function updateTaskList(s_collection, parentDom) {

    var parentList = parentDom.find("ul");
    var show = false;

    jQuery.ajax({
        type: "POST",
        url: "ViewTaskManager.aspx/" + s_collection,
        contentType: "application/json; charset=utf-8",
        dataType: "json",


        success: function (meh) {

            var list = "<li class='task'>";
            var render = true;

            jQuery(meh.d).sort(sortDate).each(function (index, el) {

                if (index == 0) {
                    if (latest != el.UniqueId) {
                        latest = el.UniqueId;
                        render = true;
                    }
                    else
                        render = false;
                }

                if (el.UniqueId == taskId)
                    showDoneMessage(el);


                //var html = "<li class='task " + el.TaskType + "  " + el.State + "'>";
                var html = "<div class='taskItem taskItem_" + el.State + "'>";

                html += "<div class='label'><h3><a href='viewRevisionDetails.aspx?revision=" + el.RevisionAlias + "'>" + el.TaskType + "</a></h3><small>" + el.FriendlyName + "</small></div>";

                if (el.Message != null)
                    html += "<div class='message'>" + el.Message + "</div>";

                html += "<br style='clear: both'/></div>";
                list += html;

                show = true;
            });
            list += "</li>";

            if (render) {
                parentList.children().remove();
                parentList.append(list);
            }
            if (show)
                parentDom.show();
            else
                parentDom.hide();

        }
    });
        }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

<umb:UmbracoPanel Text="Taskmanager" runat="server" hasMenu="false" ID="pagePanel">
<div id="feedback" style="display: none"></div>

<div id="current" style="display: none">
<h2 class="propertypaneTitel">In-progress tasks</h2>
<ul class='taskmanager'></ul>
</div>

<div id="done" style="display: none">
<h2 class="propertypaneTitel">No current tasks running</h2>
    <ul class='taskmanager'>
        <li>
               <p>
                    There are not current tasks running, you can close this window       
               </p>
        </li>
    </ul>
</div>


<div id="queue" style="display: none">
<h2 class="propertypaneTitel">Queued tasks</h2>
<ul class='taskmanager'></ul>
</div>


<div id="processed" style="display: none">
<h2 class="propertypaneTitel">Recently processed tasks</h2>
<ul class='taskmanager'></ul>
</div>
</umb:UmbracoPanel>

<asp:placeholder runat="server" id="dialogPanel" visible="false">

<div id="feedback" style="display: none"></div>

<div id="current" style="display: none">
<h2 class="propertypaneTitel">In-progress tasks</h2>
<ul class='taskmanager'></ul>
</div>


<div id="queue" style="display: none">
<h2 class="propertypaneTitel">Queued tasks</h2>
<ul class='taskmanager'></ul>
</div>


<div id="processed" style="display: none">
<h2 class="propertypaneTitel">Recently processed tasks</h2>
<ul class='taskmanager'></ul>
</div>
</asp:placeholder>

</asp:Content>
