window.addEventListener('load', _ => {
    let form = document.getElementById('form');
    let details = document.getElementById('added-detail-container');
    let input = document.getElementById('detail-input');
    let button = document.getElementById('detail-btn');
    let generateInputTag = () => {
        let input = document.createElement('input');
        input.className = "detail-added";
        input.type = "text";
        return input;
    };
    let addToDetails = (detail) => {
        if (detail !== "" || !detail) {
            let detailBox = generateInputTag();
            detailBox.value = detail;
            details.appendChild(detailBox);
        }
    }
    input.onkeydown = (e) => {
        console.log(e);
        if (e.keyCode == 13) {
            addToDetails(input.value);
            input.value = "";
            return false;
        }
    }

    button.onclick =  _=>
    {
        addToDetails(input.value);
        input.value = "";
    }

    form.onsubmit = _ => {
        let hiddenText = generateInputTag();
        hiddenText.type = "hidden";
        hiddenText.value = "";
        for (let i = 0; i < details.children.length; i++) {
            hiddenText.value += `${details.children[i.toString()].value}|`;
        }
        hiddenText.value += input.value;
        hiddenText.name = "details";

        form.appendChild(hiddenText);
        return true;
    }

});