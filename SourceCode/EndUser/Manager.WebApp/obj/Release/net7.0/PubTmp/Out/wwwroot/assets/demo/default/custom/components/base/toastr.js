var ToastrDemo=function(){var t=function(){var k,m=-1,f=0;$("#showtoast").click(function(){var t,o,e=$("#toastTypeGroup input:radio:checked").val(),n=$("#message").val(),a=$("#title").val()||"",i=$("#showDuration"),s=$("#hideDuration"),r=$("#timeOut"),l=$("#extendedTimeOut"),c=$("#showEasing"),p=$("#hideEasing"),u=$("#showMethod"),d=$("#hideMethod"),h=f++,v=$("#addClear").prop("checked");toastr.options={closeButton:$("#closeButton").prop("checked"),debug:$("#debugInfo").prop("checked"),newestOnTop:$("#newestOnTop").prop("checked"),progressBar:$("#progressBar").prop("checked"),positionClass:$("#positionGroup input:radio:checked").val()||"toast-top-right",preventDuplicates:$("#preventDuplicates").prop("checked"),onclick:null},$("#addBehaviorOnToastClick").prop("checked")&&(toastr.options.onclick=function(){alert("You can perform some custom action after a toast goes away")}),i.val().length&&(toastr.options.showDuration=i.val()),s.val().length&&(toastr.options.hideDuration=s.val()),r.val().length&&(toastr.options.timeOut=v?0:r.val()),l.val().length&&(toastr.options.extendedTimeOut=v?0:l.val()),c.val().length&&(toastr.options.showEasing=c.val()),p.val().length&&(toastr.options.hideEasing=p.val()),u.val().length&&(toastr.options.showMethod=u.val()),d.val().length&&(toastr.options.hideMethod=d.val()),v&&(t=(t=n)||"Clear itself?",n=t+='<br /><br /><button type="button" class="btn btn-outline-light btn-sm m-btn m-btn--air m-btn--wide clear">Yes</button>',toastr.options.tapToDismiss=!1),n||(++m===(o=["New order has been placed!","Are you the six fingered man?","Inconceivable!","I do not think that means what you think it means.","Have fun storming the castle!"]).length&&(m=0),n=o[m]),$("#toastrOptions").text("toastr.options = "+JSON.stringify(toastr.options,null,2)+";\n\ntoastr."+e+'("'+n+(a?'", "'+a:"")+'");');var g=toastr[e](n,a);void 0!==(k=g)&&(g.find("#okBtn").length&&g.delegate("#okBtn","click",function(){alert("you clicked me. i was toast #"+h+". goodbye!"),g.remove()}),g.find("#surpriseBtn").length&&g.delegate("#surpriseBtn","click",function(){alert("Surprise! you clicked me. i was toast #"+h+". You could perform an action here.")}),g.find(".clear").length&&g.delegate(".clear","click",function(){toastr.clear(g,{force:!0})}))}),$("#clearlasttoast").click(function(){toastr.clear(k)}),$("#cleartoasts").click(function(){toastr.clear()})};return{init:function(){t()}}}();jQuery(document).ready(function(){ToastrDemo.init()});