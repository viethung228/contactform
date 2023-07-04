const liff = window.liff;

// Body element
const body = document.getElementById('body');

// Profile elements
const userId = document.getElementById('userId');
const displayName = document.getElementById('displayName');
const email = document.getElementById('email');

async function main() {
    liff.ready.then(() => {
        if (liff.getOS() === 'android') {
            body.style.backgroundColor = '#888888';
        }
        if (liff.isInClient()) {
            getUserProfile();
        }
    });

    await liff.init({ liffId: '1657089282-RoGvdvxv' });
    
}
main();

async function getUserProfile() {
    const profile = await liff.getProfile();

    var lineUserId = profile.userId;
    var lineDisplayName = profile.displayName;
    var lineUserEmail = liff.getDecodedIDToken().email;

    //var lineUserId = "2352fw";
    //var lineDisplayName = "Tuan";
    //var lineUserEmail = "ducutan9603@gmail.com";

    userId.innerHTML = '<b>userId: </b>' + lineUserId;
    displayName.innerHTML = '<b>displayName: </b>' + lineDisplayName;
    email.innerHTML = '<b>email: </b>' + lineUserEmail;

    //Send data to database
    var frmData = [];
    frmData.push({ name: 'LineId', value: lineUserId });
    frmData.push({ name: 'DisplayName', value: lineDisplayName });
    frmData.push({ name: 'Email', value: lineUserEmail });

    $.aPost("/Liff/GetUserProfile", frmData, function () {
        
    }, "json", true);
}