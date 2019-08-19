function myFunction() {
    var tiffinType = document.getElementById('tiffinType').value;
    var extraRotiCount = document.getElementById('extraRotiCount').value;

    if (tiffinType == "" || tiffinType == null) {
        document.getElementById("error").innerHTML = "Please select tiffin type.";
    }
    else if (tiffinType == 2) {
        var finalAmount = 40 + (extraRotiCount * 5);
        document.getElementById("error").innerHTML = "Total price : " + finalAmount;
    }
    else {
        var finalAmount = 50 + (extraRotiCount * 5);
        document.getElementById("error").innerHTML = "Total price : " + finalAmount;
    }
}