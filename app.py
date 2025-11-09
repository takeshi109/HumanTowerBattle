from flask import Flask, Response
import cv2
import numpy as np
import mediapipe as mp


app = Flask(__name__)
mp_selfie_segmentation = mp.solutions.selfie_segmentation
segmenter = mp_selfie_segmentation.SelfieSegmentation(model_selection=0)

# グローバルでカメラを開いておく
cap = cv2.VideoCapture(0)

@app.route('/image')
def send_image():
    ret, frame = cap.read()
    if not ret:
        return "カメラ取得失敗", 500
    
    frame = cv2.flip(frame, 1) #フレーム反転
    #frame = software_zoom_out(frame, zoom_factor=0.5)
    results = segmenter.process(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
    mask = results.segmentation_mask > 0.5

    rgba = cv2.cvtColor(frame, cv2.COLOR_BGR2BGRA)
    rgba[..., 3] = (mask * 255).astype(np.uint8)

    _, png = cv2.imencode('.png', rgba)
    return Response(png.tobytes(), mimetype='image/png')

@app.route('/detect')
def detect_human():
    ret, frame = cap.read()
    if not ret:
        return "false"

    frame = cv2.flip(frame, 1)
    results = segmenter.process(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
    mask = results.segmentation_mask > 0.5

    if np.sum(mask) > (mask.size * 0.1):  # 10%以上が人と判定されたら
        return "true"
    else:
        return "false"

if __name__ == '__main__':
    app.run(debug=True)