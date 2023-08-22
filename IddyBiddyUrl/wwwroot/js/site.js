// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const toggleButton = document.getElementById('toggle-short-link');
const shortLinkContainer = document.getElementById('short-link-container');
const generateShortLink = document.getElementById('generate-short-link');

generateShortLink.value = true;

if (document.getElementById('short-link').value !== '') {
    shortLinkContainer.style.display = 'block';
    generateShortLink.value = false;
}

toggleButton.addEventListener('click', () => {
    let shortLinkDisplayed = shortLinkContainer.style.display === 'none'
    shortLinkContainer.style.display = shortLinkDisplayed ? 'block' : 'none';
    toggleButton.innerHTML = shortLinkDisplayed ? 'Choose your own Url' : 'Hide custom Url';
    generateShortLink.value = !shortLinkDisplayed;
});