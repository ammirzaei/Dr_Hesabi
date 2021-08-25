function CopyLink(id, title) {
    debugger;
    var domain = $("#MyDomain").val();
    var result = domain + `/Tests/${id}/${title}`;
    document.getElementById("TestLink_" + id).defaultValue = result;
    var link = document.getElementById("TestLink_" + id);
    link.select();
    link.setSelectionRange(0, 99999);
    document.execCommand("copy");
    alert("لینک با موفقیت کپی شد");
}