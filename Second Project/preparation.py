import cv2
from threading import Thread

class Webcam:

    def __init__(self):
        self.cap = cv2.VideoCapture(0)
        self.imgWebcam = self.cap.read()[1]

    # create thread for capturing images
    def start(self):
        Thread(target=self._update_frame, args=()).start()

    def _update_frame(self):
        while (True):
            self.imgWebcam = self.cap.read()[1]

    # get the current frame
    def get_current_frame(self):
        return self.imgWebcam

class Database:
    def __init__(self):
        self.imgTarget2 = cv2.imread('lord_of_rings_fellowship.jpg')
        self.imgTarget = cv2.imread('kung_pow.jpg')
        self.imgOverlay2 = cv2.imread('lord_of_rings_fellowship_text.png')
        self.imgOverlay = cv2.imread('kung_pow_text.png')
        self.imgCube = cv2.imread('cube.png')
        self.target_score = 5
        self.target2_score = 3

    def get_imgTarget(self):
        return self.imgTarget

    def get_imgTarget_overlay(self):
        return self.imgOverlay

    def get_imgTarget2(self):
        return self.imgTarget2

    def get_imgTarget_overlay2(self):
        return self.imgOverlay2

    def get_target_score(self):
        return self.target_score

    def get_target2_score(self):
        return self.target2_score

    def get_cube(self):
        return self.imgCube