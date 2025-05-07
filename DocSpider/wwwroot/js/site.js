// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const toggleBtn = document.getElementById("toggleSidebar");
const sidebar = document.getElementById("sidebar");
const mainContent = document.getElementById("mainContent");

const btnFiltro = document.getElementById("toggleFiltro");
const filtro = document.getElementById("filtro");

toggleBtn.addEventListener("click", function () {
    if (window.innerWidth < 768) {
        sidebar.classList.toggle("show");
    } else {
        sidebar.classList.toggle("collapsed");
        mainContent.classList.toggle("collapsed");
    }
});

btnFiltro.addEventListener("click", function () {
    if (window.innerWidth < 768) {
        filtro.classList.toggle("hide-filtro");
    }
});

window.addEventListener("click", function (e) {
    if (window.innerWidth < 768 && !sidebar.contains(e.target) && !toggleBtn.contains(e.target)) {
        sidebar.classList.remove("show");
    }
});
