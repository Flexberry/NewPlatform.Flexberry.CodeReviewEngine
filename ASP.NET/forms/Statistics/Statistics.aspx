<%@ Page MasterPageFile="~/Site1.Master" Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="IIS.CodeReviewEngine.forms.Statistics.Statistics" %>
<%@ Import Namespace="System.Security.Cryptography" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DropDownList id="ctrlTeam" runat="server"/>
    <div id="notCompletedResponse" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
    <div id="responseInProgress" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
    <div id="notCompletedRequest" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
    <div id="requestInProgress" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
    <div id="completedRequest" style="min-width: 400px; height: 400px; margin: 0 auto"></div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder0">
    <script type="text/javascript" src="/Scripts/urlHelper.js"></script>
    <script type="text/javascript" src="/Scripts/highcharts.js"></script>
    <script type="text/javascript">
        $(function () {
            var reloadDiagrams = function() {
                var data = <%=GetData()%>;

                $('#notCompletedResponse').highcharts({
                    chart: {
                        type: 'column'
                    },
                    colors: ["#5DB0BF"],
                    title: {
                        text: 'Рейтинг невыполненных Code Review'
                    },
                    subtitle: {
                        text: 'Число непроверенных чекинов'
                    },
                    xAxis: {
                        categories: data.namesNotCompletedResponse
                    },
                    yAxis: {
                        allowDecimals: false,
                        min: 0,
                        title: {
                            text: 'Число непроверенных чекинов (шт.)'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: 'Число непроверенных чекинов: {point.y} шт.',
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 1
                        }
                    },
                    series: [{
                        data: data.numbersNotCompletedResponse
                    }]
                });
            
                $('#responseInProgress').highcharts({
                    chart: {
                        type: 'column'
                    },
                    colors: ["#355D80"],
                    title: {
                        text: 'Рейтинг выполняемых Code Review'
                    },
                    subtitle: {
                        text: 'Число проверяемых чекинов'
                    },
                    xAxis: {
                        categories: data.namesResponseInProgress
                    },
                    yAxis: {
                        allowDecimals: false,
                        min: 0,
                        title: {
                            text: 'Число проверяемых чекинов (шт.)'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: 'Число проверяемых чекинов: {point.y} шт.',
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 1
                        }
                    },
                    series: [{
                        data: data.numbersResponseInProgress
                        }]
                }); 
                
                $('#notCompletedRequest').highcharts({
                    chart: {
                        type: 'column'
                    },
                    colors: ["#FF4C36"],
                    title: {
                        text: 'Рейтинг незакрытых Review Request'
                    },
                    subtitle: {
                        text: 'Число необработанных ответов на Code Review'
                    },
                    xAxis: {
                        categories: data.namesNotCompletedRequest
                    },
                    yAxis: {
                        allowDecimals: false,
                        min: 0,
                        title: {
                            text: 'Число необработанных ответов на Code Review (шт.)'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: 'Число необработанных ответов на Code Review: {point.y} шт.',
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 1
                        }
                    },
                    series: [{
                        data: data.numbersNotCompletedRequest
                    }]
                }); 

                $('#requestInProgress').highcharts({
                    chart: {
                        type: 'column'
                    },
                    colors: ["#404040"],
                    title: {
                        text: 'Рейтинг Review Request в работе'
                    },
                    subtitle: {
                        text: 'Число обрабатываемых ответов на Code Review'
                    },
                    xAxis: {
                        categories: data.namesRequestInProgress
                    },
                    yAxis: {
                        allowDecimals: false,
                        min: 0,
                        title: {
                            text: 'Число обрабатываемых ответов на Code Review (шт.)'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: 'Число обрабатываемых ответов на Code Review: {point.y} шт.',
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 1
                        }
                    },
                    series: [{
                        data: data.numbersRequestInProgress
                    }]
                }); 

                $('#completedRequest').highcharts({
                    chart: {
                        type: 'column'
                    },
                    colors: ["#E6E24D"],
                    title: {
                        text: 'Рейтинг закрытых Review Request'
                    },
                    subtitle: {
                        text: 'Число обработанных ответов на Code Review'
                    },
                    xAxis: {
                        categories: data.namesCompletedRequest
                    },
                    yAxis: {
                        allowDecimals: false,
                        min: 0,
                        title: {
                            text: 'Число обработанных ответов на Code Review (шт.)'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: 'Число обработанных ответов на Code Review: {point.y} шт.',
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.2,
                            borderWidth: 1
                        }
                    },
                    series: [{
                        data: data.numbersCompletedRequest
                    }]
                }); 
            };

            if (<%=ctrlTeam.Visible.ToString().ToLower()%>) {
                var ctrlTeams = $("#<%=ctrlTeam.ClientID%>");
                ctrlTeams.on('change', function() {
                    window.location.href = $.ics.urlHelper.combineUrl(window.location.href, { '<%=TeamPkParameterName%>': ctrlTeams.val() });
                });
            } 
            else {
                reloadDiagrams();
            }
    });
    </script>
</asp:Content>