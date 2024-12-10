
(function () {
    "use strict";
    var messageBanner;

    Office.initialize = function (reason) {
        $(document).ready(function () {
            var element = document.querySelector('.MessageBanner');
            messageBanner = new components.MessageBanner(element);
            messageBanner.hideBanner();

            showReloadButton();
            showBackButton();
            setTimeout(function () {
                $(document).trigger('afterready');
            }, 1);
        });
    }; 

    function getRequest() {
        $.get("http://localhost:5001/api/samsung-notes", function (data, status) {
            if (status == "success") {
                loadImages(data);
            }
        });
    }

    function getNotesPages(uuid) {
        hideReloadButton();
        showBackButton();
        $.post("http://localhost:5001/api/pages", uuid, function (data, status) {
            if (status == "success") {
                loadNotePages(data);
            }
        });
    }

    function loadImages(noteInfo) {
        const imageContainer = document.getElementById('imageGrid');
        imageContainer.innerHTML = '';
        const noteInfoList = noteInfo;
        // noteInfoList: item1 = base64 string, item2 = title, item3 = uuid
        for (let i = 0; i < noteInfoList.length; i++) {
            const imageItem = document.createElement('div');
            imageItem.classList.add('image-item');
            const ext = "data:image/png;base64,";
            const img = document.createElement('img');
            img.src = ext + noteInfoList[i].item1;
            img.alt = noteInfoList[i].item3; // uuid are stored in alternative.
            const title = document.createElement('p');
            title.className = 'image-title';
            title.textContent = noteInfoList[i].item2;
            img.addEventListener('click', function () {
                getNotesPages(img.alt);
            });
            imageItem.appendChild(img);
            imageItem.appendChild(title);
            imageGrid.appendChild(imageItem);
        }
    }

    function loadNotePages(notePagesInfo) {
        const imageContainer = document.getElementById('imageGrid');
        imageContainer.innerHTML = '';
        const notePagesInfoList = notePagesInfo;
        // noteInfoList: item1 = base64 string, item2 = title, item3 = uuid
        for (let i = 0; i < notePagesInfoList.length; i++) {
            const imageItem = document.createElement('div');
            imageItem.classList.add('image-item');
            const prefix = "data:image/png;base64,";
            const img = document.createElement('img');
            img.src = prefix + notePagesInfoList[i];
            img.alt = notePagesInfoList[i]; // base64string without prefix are stored in alternative.
            const title = document.createElement('p');
            title.className = 'image-title';
            title.textContent = "Page " + (i + 1);
            img.addEventListener('click', function () {
                getDataFromSelection(img.alt);
            });
            imageItem.appendChild(img);
            imageItem.appendChild(title);
            imageGrid.appendChild(imageItem);
        }
    }

    function showReloadButton() {
        const container = document.getElementById('reloadKey');
        container.innerHTML = '';

        const iconItem = document.createElement('img');
        iconItem.classList.add("reload");
        iconItem.src = "images/reload.png"
        container.appendChild(iconItem);

        iconItem.addEventListener('click', function () {
            getRequest();
        });
    }

    function hideReloadButton() {
        const container = document.getElementById('reloadKey');
        container.innerHTML = '';
    }

    function showBackButton() {
        const backButton = document.getElementById('backKeyDiv');
        backButton.innerHTML = '';
        const button = document.createElement('img');
        button.src = "images/back.png";
        button.id = "backButton";

        button.addEventListener("click", function () {
            hideBackButton();
            getRequest();
        });

        backButton.appendChild(button);
    }

    function hideBackButton() {
        const backButton = document.getElementById('backKeyDiv');
        backButton.innerHTML = '';
        showReloadButton();
        getRequest();
    }

    function getDataFromSelection(base64EncodedImageStr) {
        Office.context.document.setSelectedDataAsync(base64EncodedImageStr,
            {
                coercionType: Office.CoercionType.Image,
                imageLeft: 0,
                imageTop: 0,
                imageWidth: 300,
                imageHeight: 540
            },
            function (asyncResult) {
                if (asyncResult.status === Office.AsyncResultStatus.Failed) {
                    console.log("Action failed with error: " + asyncResult.error.message);
                }
            });
    }

    })
();
