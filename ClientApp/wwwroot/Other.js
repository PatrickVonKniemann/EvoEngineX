window.downloadFileFromStream = (filename, byteArray) => {
    console.log("downloadFileFromStream called");

    try {
        const blob = new Blob([new Uint8Array(byteArray)], {type: 'text/csv'});
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        URL.revokeObjectURL(link.href);
        console.log("File download triggered successfully");
    } catch (error) {
        console.error("Error in downloadFileFromStream: ", error);
    }
};
