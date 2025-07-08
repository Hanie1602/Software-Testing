// Import necessary functions from Firebase SDK
import { initializeApp } from 'https://www.gstatic.com/firebasejs/9.6.1/firebase-app.js';
import { getStorage, ref, uploadBytes, getDownloadURL } from 'https://www.gstatic.com/firebasejs/9.6.1/firebase-storage.js';

// Firebase configuration
const firebaseConfig = {
    apiKey: "AIzaSyDC1sfSIQKXTwkyGDR27LsM3AAqsSQiogk",
    authDomain: "auth-demo-123e3.firebaseapp.com",
    projectId: "auth-demo-123e3",
    storageBucket: "auth-demo-123e3.appspot.com",
    messagingSenderId: "867903141504",
    appId: "1:867903141504:web:d8f7ca0edb80a4dc04d865",
    measurementId: "G-JEWGJ20EEC"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const storage = getStorage(app);

document.getElementById('clinicForm').addEventListener('submit', function (event) {
    event.preventDefault(); // Ngăn hành động mặc định của form

    const fileInput = document.getElementById('fileInput');
    const file = fileInput.files[0];

    if (file) {
        const storageRef = ref(storage, 'profile_pictures/' + file.name);

        uploadBytes(storageRef, file).then((snapshot) => {
            console.log('Uploaded a file successfully!', snapshot);

            getDownloadURL(storageRef).then((url) => {
                console.log('File available at', url);

                const imageUrlElement = document.getElementById('imageUrl');
                imageUrlElement.value = url;

                // Tiếp tục gửi form sau khi URL đã được thiết lập
                document.getElementById('clinicForm').submit();
            }).catch((error) => {
                console.error('Error getting download URL:', error);
            });
        }).catch((error) => {
            console.error('Error uploading file:', error);
        });
    } else {
        //console.log('No file selected');
        //alert('No file selected');
        //document.getElementById('clinicForm').submit();
        form.submit();
    }
});
