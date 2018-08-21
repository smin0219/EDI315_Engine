<!--#Include Virtual="/dbopen.asp"-->
<%
If Session("agtcode")="" then
	Response.redirect "default.asp"
else	
	if Session("CompId")="KEUSA" then
		Response.Redirect "KE_default.asp"
	else
		agtcode=Session("agtcode")
	
		sql="Select * from AgentTB where agtcode='"& agtcode &"'"
		set rs=db.Execute(sql)	

        sql1="Select * from Web_Announcement where fromdate < GETDATE() and todate > GETDATE() and module = 'Casnet'"
        set announcement = db.Execute(sql1)
	end if
end if

AWBInputLink="/AWBManager/Import/Add"

Set objTypeLib	= Server.CreateObject("ScriptLet.TypeLib")
guidNew		= Left(objTypeLib.GUID, 38)
Set objTypeLib	= Nothing
%>
<html>
<title>ePicNet</title>
<style type="text/css">
    <!--
    .Noborder {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 25px;
        font-style: normal;
        border-top-width: thin;
        border-right-width: thin;
        border-bottom-width: thin;
        border-left-width: thin;
        border-top-style: none;
        border-right-style: none;
        border-bottom-style: none;
        border-left-style: none;
        color: #000000;
    }

    .hide {
        visibility: hidden;
        display: none;
    }

    .style6 {
        font-size: 25px;
        font-weight: bold;
    }

    body {
        margin-top: 0px;
        margin-left: 0px;
        margin-right: 0px;
        margin-bottom: 0px;
    }
    -->
</style>
<script type="text/javascript">
<!--
    function MM_openBrWindow(theURL, winName, features) { //v2.0
        window.open(theURL, winName, features);
    }
    //-->

    onload = function () {
        var e = document.getElementById("isPageRefreshed");
        if (e.value == "no") {
            e.value = "yes";
        }
        else {
            e.value = "no";
            location.reload();
        }
    }

    function SecurityForm_submitIt() {
        document.SecurityForm.action = "https://www.tfaforms.com/359899";
        document.SecurityForm.target = '_blank';
        document.SecurityForm.method = "POST";
        document.SecurityForm.submit();
    }
</script>
<link href="style2.css" rel="stylesheet" type="text/css">
<body>
    <input type="hidden" id="isPageRefreshed" value="no" />
    <!-- #BeginLibraryItem "/Library/top_menu.lbi" -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <img src="images/WFS_header_image.png" />
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1085" background="images/pages/top_menu_back.gif">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="145" background="images/pages/top_menu_back.gif">&nbsp;</td>
                        <td width="142" background="images/pages/top_menu_back.gif">&nbsp;</td>
                        <td width="115" background="images/pages/top_menu_back.gif">&nbsp;</td>
                        <td background="images/pages/top_menu_back.gif">&nbsp;</td>
                        <%if Cint(Session("agtlevel")) > 3 then%>
                        <td width="43" align="right" background="images/pages/top_menu_back.gif"><a href="default3.asp">
                            <img src="images/buttons/homeicon_main.gif" alt="CASnet Home" width="43" height="39" border="0"></a></td>
                        <%end if%>
                        <td width="43" background="images/pages/top_menu_back.gif"><a href="default2.asp">
                            <img src="images/buttons/homeicon.gif" alt="Cash Collection Home" width="43" height="39" border="0"></a></td>
                        <td width="45" align="right" background="images/pages/top_menu_back.gif"><a href="logout.asp">
                            <img src="images/buttons/logout.gif" alt="Log out" width="45" height="39" border="0"></a></td>
                    </tr>
                </table>
            </td>
            <td background="images/pages/top_menu_back.gif">&nbsp;</td>
        </tr>
    </table>
    <!-- #EndLibraryItem -->
    <table <%if Cint(Session("agtlevel")) >= 6 then%>width="1100" <%else%>width="900" <%end if%> border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="10" cellspacing="0">
                    <tr>
                        <td>&nbsp;</td>
                        <%if Session("lCount")>1 then%>
                        <td width="250"><span class="text12 gray66">Current Location : <%=Session("Lcode")%></span> &nbsp;&nbsp;<a href="../LocationChoose.asp" class="text12link darkred">Change Location</a></td>
                        <%end if%>
                        <%if Cint(Session("agtlevel"))=8 then%>
                        <td width="100"><a href="Ccollection/CC_problem.asp" class="text12link darkblue">Ccode problem</a></td>
                        <%end if%>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <!-- ****** Before Web_Announcement was used ****** 2016/11/17 JK
                    
                    <table width="100%" border="1" cellpadding="10" cellspacing="0" style="margin-left:15px; width:1070px; display:none;">
                    <tr>
                        <td>
                            <span class="text12 bold">Dear Agents, our Credit Card Portal is up.</span><br />
                            <span class="text12 bold">The Customers can make payments by credit card or e-check online at this moment.</span><br />
                            <span class="text12 bold">Please continue to accept checks payable to CAS for those AWBs which will be picked up today, and waive the surcharge of $10 with a waive reason "ePIC is Down". </span><br />
                            <span class="text12 bold">Thank you for your patience!</span><br /><br />
                            <span class="text12 bold">CAS Management</span>
                        </td>
                    </tr>
                </table>-->

                <% If announcement.EOF or announcement.BOF then
				else 
					Do until announcement.EOF %>
                <table width="100%" border="1" cellpadding="10" cellspacing="0" style="margin-left:15px; border-color:red; width:1070px">
                    <tr>
                        <td>
                            <!--<span class="text12 bold">IT Department is looking for desktop support talents. </span><br />
                            <span class="text12 bold">If you are interested, please send your resume to <a href="mailto:czhao@casusa.com" target="_top">czhao@casusa.com</a> or <a href="mailto:schoi@casusa.com" target="_top">schoi@casusa.com</a> by Nov 15th. </span><br />-->
                            
                            
                            <span class="text14 bold"><%=announcement("Contents") %></span><br />




                            <!--<span>Feel free to request detailed Role and Responsibilities to ckim@casusa.com</span><br />-->
                        </td>
                    </tr>
                </table>
                <% announcement.MoveNext
				Loop
				End if
				announcement.Close
				%>



                <table border="0" align="center" cellpadding="0" cellspacing="0" <%if Cint(Session("agtlevel")) >= 6 then%>width="1066" <%else%>width="800" <%end if%>>
                    <tr>
                        <td valign="top" background="images/pages/menu_box_back.gif">
                            <img src="image/transparent.gif" width="100" height="8"></td>
                        <td valign="top" background="images/pages/menu_box_back.gif">
                            <img src="image/transparent.gif" width="100" height="8"></td>
                        <td valign="top" background="images/pages/menu_box_back.gif">
                            <img src="image/transparent.gif" width="100" height="8"></td>
                        <%if Cint(Session("agtlevel")) >= 6 then%>
                        <td valign="top" background="images/pages/menu_box_back.gif">
                            <img src="image/transparent.gif" width="100" height="8"></td>
                        <%end if%>
                    </tr>
                    <tr>
                        <td width="266">
                            <img src="images/titles/cash_collection.gif" width="250" height="45"></td>
                        <td width="266" class="text22 darkblue bold">
                            <img src="images/titles/operation.gif" width="250" height="45"></td>
                        <td width="266" class="text22 darkblue bold">
                            <img src="images/titles/shipment_manage.gif" width="250" height="45"></td>
                        <%if Cint(Session("agtlevel")) >= 6 then%>
                        <td width="266" class="text22 darkblue bold">
                            <img src="images/titles/month_closing.gif" width="250" height="45"></td>
                        <%end if%>
                    </tr>
                    <tr>
                        <td width="266" valign="top" background="images/pages/menu_box_back.gif">
                            <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                    <td class="text12 bold">Cash Collection</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                            <tr>
                                                <td width="15" align="right" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="/CashCollection" class="text12link skyblue">New Input Screen</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="#" class="text12link skyblue" onclick="MM_openBrWindow('Ccollection/reprint.asp','Receipt','toolbar=yes,scrollbars=yes,width=800,height=650')">Print Receipt</a></td>
                                            </tr>
                                            <%if Cint(Session("agtlevel")) >= 6 then%>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="Ccollection/blacklist/list.asp" class="text12link darkred">Managing (Black List)</a></td>
                                            </tr>
                                            <%end if%>
                                            <%if Cint(Session("agtlevel")) >= 8 then%>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="Ccollection/managelist.asp" class="text12link darkred">Managing (Detail / Edit / Delete)</a></td>
                                            </tr>
                                            <%end if%>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <%if Session("agtcode")="albert" then%>
                                <tr>
                                    <td class="text12 bold">Save payment</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                            <tr>
                                                <td width="15" align="right" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="Ccollection/payment/CC_prepay.asp" class="text12link skyblue">Save ISC Pre Payment</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" align="right" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="Ccollection/payment/CC_prepay_list.asp" class="text12link skyblue">Save Payment History</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <%end if%>
                            </table>
                            <%if Cint(Session("agtlevel")) >= 3 then%>
                            <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="text12 bold">Daily Post</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                            <tr>
                                                <td width="15" align="right" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td class="text12 gray66"><a href="dp/dailycash.asp" class="text12link gray66">Daily post screen</a><br />
                                                    <strong>(**BEFORE 10/14/2010)</strong></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="/DailyPost/Home" class="text12link skyblue">Post history</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="dp/Coupon_List.asp" class="text12link skyblue">Coupon Post history</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="dp/hold_list.asp" class="text12link skyblue">Hold list</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" align="right" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td class="text12"><a href="/DailyPost/DailyCash/Post" class="text12link darkred">New Daily post</a><br />
                                                    <strong>(**AFTER 10/14/2010)</strong></td>
                                            </tr>
                                            <%if Cint(Session("agtlevel")) >= 6 then%>
                                            <tr>
                                                <td width="15" align="right" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="/ePicManager" class="text12link darkblue bold">ePic Accounting Management</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="dp/fix_post1.asp" class="text12link darkred">Fix "N/A" post</a></td>
                                            </tr>
                                            <%end if%>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                            </table>
                            <%end if%>
                            <%if Cint(Session("agtlevel")) >= 3 or Session("AgtBilling")=1 then%>
                            <br>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="images/titles/billing_process.gif" width="250" height="45"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <%if Cint(Session("agtlevel")) >= 3 or Session("AgtBilling")=1 then%>
<!--                                            <tr>
                                                <td class="text12 bold"><a href="Billing/new.asp" class="text12link skyblue bold">Input Screen</a></td>
                                            </tr>-->

                                            <tr>
                                                <td class="text12 bold">
                                                    
                                                    <a href="/Report/BillingReport/NewBilling" class="text12link skyblue bold">Input Screen</a>
                                                    <!--<span style="color:red;" class="text12link bold">(DO NOT USE)</span>-->


                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
<!--                                            <tr>
                                                <td class="text12 bold"><a href="Billing/list.asp" class="text12link skyblue bold">Review &amp; Adjust</a></td>
                                            </tr>-->

                                            <tr>
                                                <td class="text12 bold"><a href="/Report/BillingReport/" class="text12link skyblue bold">Review &amp; Adjust</a>
                                                    <!--<span style="color:red;" class="bold text12link">(DO NOT USE)</span>-->
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <%end if%>
                                            <%if Cint(Session("agtlevel")) >= 6 then%>
                                            <tr>
                                                <td><a href="Bill_Report/new.asp" class="text12link skyblue bold">Design Billing Stats Reports</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td><a href="/MasterTable/BillingSapInfo/BillingSapInfoList" class="text12link skyblue bold">Billing Sap Information</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td><a href="/MasterTable/BillingFrequency/BillingFrequencyList" class="text12link skyblue bold">Billing Sap Frequency Settings</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <%end if
				  if Cint(Session("agtlevel")) >= 3 then%>
                                            <!--<tr>
                                                <td><a href="Bill_Report/index.asp" class="text12link skyblue bold">Billing Stats Report by customer</a></td>
                                            </tr>-->

                                            <tr>
                                                <td><a href="/Report/BillingReport/BillingReportSelect/" class="text12link skyblue bold">Billing Stats Report by customer</a>
                                                    <!--<span style="color:red;" class="bold text12link">(DO NOT USE)</span>-->
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>

<!--                                            <tr>
                                                <td><a href="Bill_Report/index.asp" class="text12link skyblue bold">Billing Stats Report by customer (Old report)</a></td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>-->
                                            <%end if%>
                                            <tr>
                                                <td><a href="Report/BillingSapLog/Index" class="text12link skyblue bold">Billing Stats Upload Log</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td><a href="Report/BillingSearchContractNo" class="text12link skyblue bold">Billing Search Contract No</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <%end if%>
                            <%if Cint(Session("agtlevel")) >= 2 then%>
                          <br>
                          <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td height="45" class="text18 bold darkblue"><img src="images/titles/Safety.gif" width="250" height="45"></td>
                            </tr>
                            <tr>
                              <td><table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=SF_Incident" class="text12link skyblue bold">Incidents</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=SF_OSHA" class="text12link skyblue bold">OSHA</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=SF_Alert" class="text12link skyblue bold">Safety Alerts</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=SF_Report" class="text12link skyblue bold">Meetings, Reports &amp; Audits</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=SF_Manual" class="text12link skyblue bold">Safety Manuals</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <form action="" method="post" name="SecurityForm">
                                  <%					
                                                agtcode=Session("agtcode")
                                                SLcode=Session("Lcode")
                                                
                                                'response.Write agtcode & "<br>"
                                                'response.Write SLcode & "<br>"
                                                'Response.End()
                                                
                                
                                                sqlInfo="Select a.idnum as idnum, Max(a.agtname) as agtname, Max(a.rot) as rot, Max(b.Acode) as Acode, Max(b.bldgnum) as bldgnum" &_
                                                " from AgentTB a, Location b, AgentLocation c where a.idnum=c.idnum and b.Lcode=c.Lcode and a.agtcode='"& agtcode &"' and b.Lcode='"& SLcode &"' and not a.agtlevel is Null and a.Active=1 group by a.idnum "
                                                set rsInfo=db.Execute(sqlInfo)
                                                
                                                Set dbrecInfo=Server.CreateObject("ADODB.Recordset")
                                                dbrecInfo.CursorType=1
                                                
                                                dbrecInfo.Open "Select a.email as email from AgentTB a, Location b, AgentLocation c where a.idnum=c.idnum and b.Lcode=c.Lcode and b.Lcode='"& SLcode &"' and a.rot like '%GM%' and a.Active=1", db
                                                
                                                GMemail=""
                                                i=1
                                                Do until dbrecInfo.EOF
                                                
                                                  if i=1 or GMemail="" then
                                                    GMemail=dbrecInfo("email")
                                                  else
                                                    if not IsNull(dbrecInfo("email")) and not IsEmpty(dbrecInfo("email")) and not dbrecInfo("email")="" then
                                                        GMemail=GMemail + ";" + dbrecInfo("email")
                                                    end if
                                                  end if
                                                
                                                dbrecInfo.MoveNext
                                                i=i+1
                                                Loop
                                                %>
                                  <tr>
                                    <td class="text12"><a href="javascript:void(0)" OnClick="SecurityForm_submitIt();return false;" target="_blank" class="text12link skyblue bold">Safety Forms
                                      <input name="tfa_636" type="hidden" id="tfa_636" value="<%=rsInfo("agtname")%>">
                                      <input name="tfa_975" type="hidden" id="tfa_975" value="<%=rsInfo("rot")%>">
                                      <input name="tfa_2" type="hidden" id="tfa_2" value="<%=rsInfo("Acode")%>">
                                      <input name="tfa_781" type="hidden" id="tfa_781" value="B<%=rsInfo("bldgnum")%>">
                                      <input name="tfa_1209" type="hidden" id="tfa_1209" value="<%=GMemail%>">
                                    </a></td>
                                  </tr>
                                </form>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                              </table></td>
                            </tr>
                          </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="45" class="text18 bold darkblue"><img src="images/titles/training.gif" width="250" height="45"></td>
                              </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=TR_Manual" class="text12link skyblue bold">Manuals</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=TR_Lesson" class="text12link skyblue bold">Lesson Plans</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=TR_Material" class="text12link skyblue bold">Training Material</a></td>
                                            </tr>
                                            <tr>
                                                <td><img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=TR_Employee" class="text12link skyblue bold">Employee Orientation</a></td>
                                            </tr>
                                            <tr>
                                                <td><img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=TR_Rules" class="text12link  skyblue bold">Rules &amp; Regulations</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=TR_Audit" class="text12link  skyblue bold">Audits &amp; Reports</a></td>
                                            </tr>
                                            <tr>
                                                <td><img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="45" class="text18 bold darkblue"><img src="images/titles/Forms.gif" width="250" height="45"></td>
                              </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=FT_OP" class="text12link skyblue bold">Operation Forms</a></td>
                                            </tr>
                                            <tr>
                                                <td><img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                              </table>
                          <%end if%>
                             <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="45" class="text18 bold darkblue"><img src="images/titles/Purchasing.gif" width="250" height="45"></td>
                              </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="mboard/list.asp?board=PC_SUP" class="text12link skyblue bold">Supplies</a></td>
                                            </tr>
                                            <tr>
                                                <td><img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            </td>
                        <td width="266" valign="top" background="images/pages/menu_box_back.gif">
                            <table width="100%" border="0" cellspacing="1" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                    <td height="25"><span class="text14 bold">&nbsp;&nbsp;Import</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold">Breakdown Reports</td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/Breport/list.asp" class="text12link skyblue">Breakdown Control List</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/TransManifest/list.asp" class="text12link skyblue">Transfer Manifest</a></td>
                                                        </tr>
                                                        <%if Cint(Session("agtlevel")) >= 8 then %>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="#" class="text12link skyblue">Daily G.O. List by Airline, by OU</a></td>
                                                        </tr>
                                                        <%end if %>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold">Carrier Certificate</td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="<%=AWBInputLink%>" class="text12link skyblue" target="_blank">AWB Input Tool</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/CarrierCert/list.asp" class="text12link skyblue">C.Certificate List</a></td>
                                                        </tr>
                                                        <%if Cint(Session("agtlevel")) >= 8 then %>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/CarrierCert/new.asp" class="text12link skyblue">Print Carrier Certificate</a></td>
                                                        </tr>
                                                        <%end if %>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="/AN" class="text12link skyblue">Auto Notification</a></td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="MODreport/list.asp" class="text12link skyblue bold">MOD Shift Report</a></td>
                                            </tr>
                                            <%if Session("WHagent")="Yes" then%>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="Agent/list.asp?pmode=WHlevel" class="text12link skyblue bold">Warehouse Agent Manage</a></td>
                                            </tr>
                                            <%end if%>
                                        </table>
                                    </td>
                                </tr>
                                <%'if Session("Lcode")="135" or Session("Lcode")="165" or Session("Lcode")="100" or Session("Lcode")="145"  or Session("Lcode")="460" then%>
                                <tr>
                                    <td height="25"><span class="text14 bold">&nbsp;&nbsp;Export</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold">Export Acceptance</td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                             <td><a href="/Acceptance/Home" class="text12link skyblue">Acceptance</a></td>
                                                        </tr>
                                                        <% if Session("Lcode")="125" or Session("Lcode")="135" or Session("Lcode")="145" or Session("Lcode")="165" or Session("Lcode")="170" or Session("Lcode")="175" or Session("Lcode")="355" or Session("Lcode")="730" or Session("Lcode")="808" or Session("Lcode")="902" or Session("Lcode")="903" or Session("Lcode")="924" or Session("Lcode")="926" then %>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/Acceptance/Home/TruckAcceptanceIndex" class="text12link skyblue">Acceptance Truck</a></td>
                                                        </tr>
                                                        <% end if %>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%'end if%>
                                <tr>
                                    <td height="25"><span class="text14 bold">&nbsp;&nbsp;IATA Message control</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <!--<td class="text12"><a href="IATA_MSG/History/list.asp" class="text12link skyblue">Message History & Manage</a></td>-->
                                                            <td class="text12"><a href="/MessageHistory/Home" class="text12link skyblue">Message History & Manage</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/ExpFlightMgmt/sendSITAmsg" class="text12link skyblue">Send SITA free text Message</a></td>
                                                        </tr>

                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="#" onclick="window.open('/FlightBookingRequest', 'FBR', 'height=800, width=890, scrollbars=yes')" class="text12link skyblue">Flight Booking Request</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%if (Session("ePic_AWBEdit") = 1 or Session("CC_ReceiptMgmt") = 1 or Session("AcceptMgmt") = 1) then%>
                                <tr>
                                    <td height="25"><span class="text14 bold">&nbsp;&nbsp;Support Tool</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td class="text12"><a href="/SupportTool" class="text12link skyblue">Support Tool</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <%end if%>

                                <tr>
                                    <td height="25"><span class="text14 bold">&nbsp;&nbsp;Truck Flight</span></td>
                                </tr>

                                <!--Truck Flight added 2015-08-18-->
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td class="text12"><a href="/Truckflight/home" class="text12link skyblue">Truck Flight</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <!-- Trucker Waiting Tier Notification added 2016-05-26 -->
                                <%if Session("agtcode")="mserzo" or Session("agtcode")="mivanova" or Session("agtcode")="czhao" or Session("agtcode")="ckim" or Lcase(Session("agtcode")) = "mmcdonald" or Lcase(Session("agtcode")) = "jzhang" then%>

                                <tr>
                                    <td height="25"><span class="text14 bold">&nbsp;&nbsp;Trucker Waiting Notification</span></td>
                                </tr>

                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td class="text12"><a href="/TruckerWaitingNotification" class="text12link skyblue">Email Set-up Tool</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%end if %>


                                <%if Cint(Session("agtlevel")) >= 1 then %>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="/FFMManager/Home/AssignInOut" class="text12link skyblue bold">Assign Outbound</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%end if %>
                            </table>

                            <br>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="images/titles/reporting_manage.gif" width="250" height="45"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <%if Cint(Session("agtlevel")) >= 3 then%>
                                            <tr>
                                                <!--<td class="text12 bold"><a href="Reports/flash/flash0.asp" class="text12link skyblue bold">Flash Report</a></td>-->
                                                <td class="text12 bold"><a href="Report/FlashReport" class="text12link skyblue bold">Flash Report</a></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold"><a href="Reports/bi_design.asp" class="text12link skyblue bold">Design Billable Item for Flash Report</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <%end if%>
                                            <tr>
                                                <td class="text12 bold"><a href="javascript:if(window.open('Operation/shipment/exp/Inventory_report_pop.asp?Lcode=<%=Session("Lcode")%>&agtCode=<%=Session("agtcode")%>','_pop','width=520,height=500'));" class="text12link skyblue bold">Inventory Report</a></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold"><a href="javascript:if(window.open('Report/ExportLocation','_pop','width=500,height=450'));" class="text12link skyblue bold">Location Report</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold"><a href="javascript:if(window.open('Operation/shipment/exp/Pallet_report_pop.asp?Lcode=<%=Session("Lcode")%>&agtCode=<%=Session("agtcode")%>','_pop','width=520,height=400'));" class="text12link skyblue bold">Gave out Pallet Report</a></td>
                                            </tr>
                                            <%if Cint(Session("agtlevel")) >= 8 or Lcase(Session("agtcode")) = "kwang" or Lcase(Session("agtcode")) = "lantico" or Lcase(Session("agtcode")) = "mvillanueva" or Lcase(Session("agtcode")) = "jeck" or Lcase(Session("agtcode")) = "rrimbaut" or Lcase(Session("agtcode")) = "rlee" or Lcase(Session("agtcode")) = "jarmstrong" then%>
                                            <tr>
                                                <td class="text12 bold"><a href="javascript:if(window.open('Report/Dimwgt/Popup?agtCode=<%=Session("agtcode")%>','_pop','width=520,height=450'));" class="text12link skyblue bold">Dim/Wgt Report</a></td>
                                            </tr>
                                            <%end if%>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>

                                            <!--<tr>
                                                <td class="text12 bold"><a href="/SecurityReport" class="text12link skyblue bold">Security Report</a></td>
                                            </tr>-->
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>

                                            <tr>
                                                <td class="text12 bold"><a href="javascript:if(window.open('Operation/shipment/exp/Daily_TruckerPickup_pop.asp?Lcode=<%=Session("Lcode")%>&agtCode=<%=Session("agtcode")%>','_pop','width=520,height=400'));" class="text12link skyblue bold">Daily Trucker Pickup Log</a></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold"><a href="/Report/TruckerWaiting" class="text12link skyblue bold">Trucker Waiting Report</a></td>
                                            </tr>
                                            <!--<tr>
                      <td class="text12 bold"><a href="/CashCollection/View/CCMainList.aspx" class="text12link skyblue bold">Cash Collection</a></td>
                    </tr>-->


                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <%if Cint(Session("agtlevel")) >= 2 then%>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td height="45" class="text18 bold darkblue"><img src="images/titles/Policy_Procedures.gif" width="250" height="45"></td>
                            </tr>
                            <tr>
                              <td><table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=PP_CompMan" class="text12link skyblue bold">Company Manuals</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=PP_Company" class="text12link skyblue bold">Company Policy</a></td>
                                </tr>
                                
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=PP_SOP" class="text12link skyblue bold">SOP</a></td>
                                  
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                  <td class="text12"><a href="mboard/list.asp?board=PP_Process" class="text12link skyblue bold">Process Analysis</a></td>
                                </tr>
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                              </table></td>
                            </tr>
                          </table>
                          <%end if%>                            
                        </td>
                        <td valign="top" background="images/pages/menu_box_back.gif">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                    <td height="25" class="text14 bold">&nbsp;&nbsp;Import</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold">Shipment Data</td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/AWBManager" class="text12link skyblue">Import AWB Management</a></td>
                                                        </tr>
                                                        <!--<tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/Shipment/list.asp" class="text12link skyblue">Import AWB Management</a></td>
                                                        </tr>-->
                                                        <%if Cint(Session("agtlevel")) >= 8 then %>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/Shipment/issueList.asp" class="text12link skyblue">Unidentified shipment</a></td>
                                                        </tr>
                                                        <%end if %>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/ImpFlightMgmt/FltMgmt/importFltMgmt_main.aspx" class="text12link skyblue">Import Flight Manager</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/ImpFlightMgmt/Dashboard" class="text12link skyblue">Import Flight DashBoard</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/Operation/Shipment/liveTally.asp" class="text12link skyblue">Live Tally Management</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <%if Cint(Session("agtlevel")) >= 8 then %>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold">Nomination</td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/Nomination/list.asp" class="text12link skyblue">Nomination Requested List</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/Nomination_CASemail/list.asp" class="text12link skyblue">CAS Agent email List</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <%end if %>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25" class="text14 bold">&nbsp;&nbsp;Export</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="1">
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold">Shipment Data</td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/AWBManager?mode=export" class="text12link skyblue">Export AWB Management</a></td>
                                                        </tr>
                                                        <!--<tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Operation/Shipment/exp/list.asp" class="text12link skyblue">Export AWB Management</a></td>
                                                        </tr>-->
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="#" onclick="return __navigateByHttpPostMethod('/ExpFlightMgmt/View/Main.aspx');" class="text12link skyblue" target="_blank">Export Flight Manager</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/ExpFlightDashboard" class="text12link skyblue">Export Flight DashBoard</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/Operation/Shipment/Exp/liveTally.asp" class="text12link skyblue">Acceptance - Live Tally Management</a></td>
                                                        </tr>

                                                        <!--<tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td></td>
                                                        </tr>-->
                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%if Cint(Session("agtlevel")) >= 3  and rs("HRlevel") >= 2 then%>
                            <br>
                            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <img src="images/titles/HR_manage.gif" width="250" height="45"></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="Applicant/loc.asp" target="_blank" class="text12link skyblue bold">New Application</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12"><a href="Applicant/list.asp" class="text12link skyblue bold">Application Manage</a></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%end if%>
                            <br />
                            <%if Cint(Session("agtlevel")) >= 2 then%>
                            <!--security bulletin-->
                          <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td height="45" class="text18 bold darkblue"><img src="images/titles/Security.gif" width="250" height="45"></td>
                            </tr>
                            <tr>
                              <td><table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                  <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                <td class="text12"><a href="mboard/list.asp?board=SC_Manual">Manual</a></td>
                                </tr>
                                <tr>
                                <td class="text12"><a href="mboard/list.asp?board=SC_Bulletin">Security Documentation</a></td>
                                </tr>
                                <tr>
                                <td class="text12 bold"><a href="/SecurityReport" class="text12link skyblue bold">Security Report</a></td>
                                </tr>

                                  <tr>
                                <td class="text12 bold"><a href="/SecurityReport" class="text12link skyblue bold">Security Report</a></td>
                                </tr>
                                  <tr>
                                <td class="text12 bold"><a href="/SecurityReport" class="text12link skyblue bold">Security Report</a></td>
                                </tr>

                                  <tr>
                                <td class="text12 bold"><a href="/SecurityReport" class="text12link skyblue bold">Security Report</a></td>
                                </tr>

                                  <tr>
                                <td class="text12 bold"><a href="/SecurityReport" class="text12link skyblue bold">Security Report</a></td>
                                </tr>

                                <tr>
                                <td><img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                              </table></td>
                            </tr>
                          </table>
                            <%end if %>

                            <br />
                            <%if Cint(Session("agtlevel")) >= 4 or (Cint(Session("agtlevel")) = 3 and rs("WWTPayrollAccess") = 1) then %>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="images/titles/labor-tracking.gif" width="250" height="45"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="/LaborTracking/WWT/List" class="text12link skyblue bold">Weekly Workforce Tracking</a></td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="/LaborTracking/WWT/PayrollAnalysis" class="text12link skyblue bold">Payroll Analysis</a></td>
                                            </tr>

                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"></td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"></td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"></td>
                                            </tr>
                                            <tr>
                                                <td bgcolor="#FFFFFF" class="text12 bold"><a href="/MasterTable/ResetPassword/SearchAgent" class="text12link skyblue bold">Reset Password</a></td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <%end if %>


                            <% If ucase(Session("Agtcode"))="TSILIGA" or ucase(Session("Agtcode"))="TOOSTENBRUG" then %>
                            <br />
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="images/titles/administration.gif" width="250" height="45"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                                <tr>
                                            <tr>
                                                <td class="text12 bold">Master Table</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Agent/list.asp?pmode=agtlevel" class="text12link skyblue">Agent</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/Flight/FlightList" class="text12link skyblue">Flight Information</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <% end if %>
            
                        </td>
                        <%if Cint(Session("agtlevel")) >= 6 then%>
                        <td valign="top" background="images/pages/menu_box_back.gif">
                            <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <img src="image/transparent.gif" width="100" height="3"></td>
                                </tr>
                                <tr>
                                    <td><span class="text12 bold">Journalize</span></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12"><a href="monthend/je.asp" class="text12link gray66">Journalizing (Cash Collection)</a></td>
                                </tr>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12 bold"><a href="monthend/Billing_close.asp" class="text12link skyblue bold">Close Billing Input</a></td>
                                </tr>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12"><a href="monthend/je_report.asp" class="text12link gray66">Journalizing (Cash Collection)</a></td>
                                </tr>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12 bold"><a href="/JournalEntry" class="text12link darkred">Journalizing (Cash Collection)</a></td>
                                </tr>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12 bold"><a href="/ePicJournalEntry" class="text12link darkred">Journalizing (ePic)</a> by Carrier</td>
                                </tr>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12 bold"><a href="/ePicJournalEntry/Home/ByUser" class="text12link darkred">Journalizing (ePic)</a> by User</td>
                                </tr>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12"><a href="monthend/je_report_month.asp" class="text12link skyblue">Posted Different Month</a></td>
                                </tr>

                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12"><a href="javascript:if(window.open('/JournalEntry/CashImport/CI','_pop','width=520,height=400'));" class="text12link skyblue">CASH Import</a></td>
                                </tr>
                                <%if Cint(Session("agtlevel")) >= 7 then%>
                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12"><a href="monthend/tonnage_report.asp" class="text12link skyblue">Create tonnage upload report</a></td>
                                </tr>

                                
                                <%end if%>

                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12"><a href="/Report/BillingTonnage" class="text12link skyblue">Rev. tonnage report</a></td>
                                </tr>

                                <tr>
                                    <td width="15" class="gray66">
                                        <li></li>
                                    </td>
                                    <td class="text12 bold"><a href="/JournalEntry/WFSJE" class="text12link darkred">WFS Journalizing</a></td>
                                </tr>
                                <tr>
                                    <td class="text12 bold">&nbsp;</td>
                                    <td class="text12 bold">&nbsp;</td>
                                </tr>
                            </table>
                            <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="text12 bold">Bank Setup</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="monthend/BkMasterL.asp" class="text12link skyblue">Bank Master</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="monthend/BankGL_list.asp" class="text12link skyblue">Bank, Payment Type & GL Account</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="monthend/BankAsgn_list.asp" class="text12link skyblue">Assign Bank to Airport</a></td>
                                            </tr>
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="monthend/giveback_list.asp" class="text12link skyblue">Giveback Setup</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <%if Cint(Session("agtlevel")) >= 4 and rs("AssignRegion") = 1then%>
                            <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td class="text12 bold">CFP</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                            <tr>
                                                <td width="15" class="gray66">
                                                    <li></li>
                                                </td>
                                                <td><a href="CFPAgentManaging/View/CFPMain.aspx" class="text12link skyblue">Assign Region</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%end if%>



                            <%if Cint(Session("agtlevel")) >= 8 then%>
                            <br>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <img src="images/titles/administration.gif" width="250" height="45"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="5"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">Web Announcement</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Web_Announcement/list.asp" class="text12link skyblue">Web Announcement</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                                <tr>
                                                    <td><a href="mtables/structure.asp" class="text12link skyblue bold">Master Table Structure</a></td>
                                                </tr>
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">Master Table</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=15" class="text12link darkred bold">Company (CAS Multi Company)</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=16" class="text12link darkred bold">Region Setup</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=1" class="text12link skyblue">Customer</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/Location/LocationList" class="text12link skyblue">Location</a></td>
                                                        </tr>

                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Agent/list.asp?pmode=agtlevel" class="text12link skyblue">Agent</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/Flight/FlightList" class="text12link skyblue">Flight Information</a></td>
                                                        </tr>

                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=6" class="text12link skyblue">Port Fee and Tax</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=11" class="text12link skyblue">Consignee</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=12" class="text12link skyblue">Commodity</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="EpicAirport" class="text12link skyblue">Airport</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <!--Added. 2017-03-10-->
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">eScreen Management</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/ScreeningUser/ScreeningUserList" class="text12link skyblue">eScreen User</a></td>
                                                        </tr>
                                                        <!--<tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/ScreeningLocation/ScreeningLocationList" class="text12link skyblue">eScreen Location</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/ScreeningCustomer/ScreeningCustomerList" class="text12link skyblue">eScreen Customer</a></td>
                                                        </tr>-->
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/ScreeningArea/ScreeningAreaList" class="text12link skyblue">eScreen Area</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <!--END-->

                                        </table>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="5"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">Cash Collection</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=3&cate=Cash" class="text12link skyblue">Billable Activity for Cash Collection</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="MasterTable/Pricing/PricingList" class="text12link skyblue">Pricing</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=17" class="text12link skyblue">Storage Rule Setup</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=9" class="text12link skyblue">Manage waive reasons</a></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="mtables/list.asp?tid=10" class="text12link skyblue">Define waive reason by Location</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="5"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">Billing </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/MasterTable/BillActCust/BillActCustList" class="text12link skyblue">Billable Activity for Billing by Customer</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>


                                        </table>
                                        <table width="90%" border="0" align="center" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="image/transparent.gif" width="100" height="5"></td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">Report by Carrier </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="Acct_Report/list.asp" class="text12link skyblue">ISC,Storage Revenue</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td class="text12"><a href="Acct_Report/list2.asp" class="text12link skyblue">ISC,Storage Revenue</a> old</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="text12 bold">FFM Manager </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/FFMManager/Home/DeleteFFM" class="text12link skyblue">Delete FFM</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <!--<tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/FFMManager/Home/AssignInOut" class="text12link skyblue">Assign Outbound</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>-->
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/FFMManager/Home/UnscheduleFlight" class="text12link skyblue">Unschedule Flight Manager</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/FFMManager/Home/EditFFM" class="text12link skyblue">Edit FFM</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text12 bold">Flight Schedule</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                        <tr>
                                                            <td width="15" class="gray66">
                                                                <li></li>
                                                            </td>
                                                            <td><a href="/FlightSchedule" class="text12link skyblue">Flight Schedule</a></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <%end if%></td>
                        <%end if%>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>
    <!-- #BeginLibraryItem "/Library/footer.lbi" -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="900" background="images/pages/footer_back1.gif">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="541">
                            <img src="images/pages/footerimg1.gif" width="541" height="41"></td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </td>
            <td background="images/pages/footer_back1.gif">&nbsp;</td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="900" background="images/pages/footer_back2.gif">
                <table width="900" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="61" background="images/pages/footer_back2.gif">
                            <table width="80%" border="0" cellspacing="0" cellpadding="3">
                                <tr>
                                    <td class="copyright">
                                        <div align="right">Copyright &copy; 2018 ePicNet.epicxp.com, All rights are reserved</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td background="images/pages/footer_back2.gif">&nbsp;</td>
        </tr>
    </table>
    <!-- #EndLibraryItem -->
    <br>
    <p>&nbsp;</p>

    <script type='text/javascript'>
        //<![CDATA[

        var __navigateByHttpPostMethod = function (/*string*/url) {

            var _form = window.document.forms["__subform__"];

            _form.method = 'post'
            _form.action = url;
            _form.submit();

            return false;
        };

        //]]>
    </script>

    <form id='__subform__' name='__subform__'>
        <input name='AgtCode' type='hidden' value='<%=session("agtcode")%>' />
        <input name='AgtName' type='hidden' value='<%=session("agtname")%>' />
        <input name='AgtLevel' type='hidden' value='<%=session("agtlevel")%>' />
        <input name='LCode' type='hidden' value='<%=session("Lcode")%>' />
    </form>

</body>
</html>
