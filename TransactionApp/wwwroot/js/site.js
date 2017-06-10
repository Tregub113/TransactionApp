window.onload = () => {
    var input = document.getElementById('exampleInputFile');
    var btn = document.getElementById('submitBtn');
    var table = document.getElementById('customers');
    var file = null;
    var mockedResult = {
        failureRow: 981,
        successRow: 18
    };

    var appendResToTable = (res) => {
        var tr = document.createElement('tr');
        var failsTd = document.createElement('td');
        var successTd = document.createElement('td');

        failsTd.innerHTML = res.failureRow;
        successTd.innerHTML = res.successRow;
        tr.appendChild(failsTd);
        tr.appendChild(successTd);
        var tbody = table.querySelector('tbody');
        tbody.appendChild(tr);
    }

    var upload = (body) => {
        var fd = new FormData();
        fd.append('file', body);
        

        window.x = fd;
        fetch('http://localhost:51294/api/Transaction/upload', {
            method: 'POST',
            headers: {
                "Content-Type": "multipart/form-data"
            },
            body: fd
        })
            .then(
            response => response.json()
            ).then(appendResToTable)
            .catch(
            error => console.error('kekus', error)
            );
    };

    var onSelectFile = ({ target }) =>
        file = target.files[0];

    input.onchange = onSelectFile;
    btn.onclick = (e) => {
        e.preventDefault();
        upload(file);
    };

}