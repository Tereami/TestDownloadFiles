<?php

$password = "unknownPass";
$filename = "unknownFile";

//ïğîâåğÿş ïàğîëü è èìÿ ôàéëà
if (isset($_POST['password']) && isset($_POST['filename'])) {
    $password = strip_tags($_POST['password']);
    $filename = strip_tags($_POST['filename']);
}

if ($password != "pass") {
    echo "error incorrect password";
    return;
}

$filepath = "download/" . $filename;

if (!file_exists($filepath)) {
    echo "error file dont exists";
    return;
}




try {
    if (ob_get_level()) {
        ob_end_clean();
    }
    //çàïîëíÿş headers
    header('Content-Description: File Transfer');
    header('Content-Type: application/octet-stream');
    header('Content-Disposition: attachment; filename=' . basename($filepath));
    header('Content-Transfer-Encoding: binary');
    header('Expires: 0');
    header('Cache-Control: must-revalidate');
    header('Pragma: public');
    header('Content-Length: ' . filesize($filepath));
    //îòïğàâëÿş ôàéë
    readfile($filepath);
    exit;
} catch (Exception $ex) {
    echo $ex->getMessage();
}
