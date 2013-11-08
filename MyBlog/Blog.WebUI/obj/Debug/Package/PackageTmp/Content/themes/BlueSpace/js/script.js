$(document).ready(function () {
    $("ul.sub-menu").parent().append("<span></span>");
    $("ul.menu li span").mouseover(function () { $(this).parent().find("ul.sub-menu").slideDown('fast').show(); $(this).parent().hover(function () { }, function () { $(this).parent().find("ul.sub-menu").slideUp('slow'); }); }).hover(function () { $(this).addClass("subhover"); }, function () { $(this).removeClass("subhover"); })
});