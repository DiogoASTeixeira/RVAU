import cv2
import numpy as np
from preparation import Webcam, Database
import sys

debug = False

if len(sys.argv) == 2:
    if str(sys.argv[1] == "debug"):
        debug = True

webcam = Webcam()
webcam.start()

database = Database()

imgTarget = database.get_imgTarget()
imgTarget2 = database.get_imgTarget2()
imgOverlay = database.get_imgTarget_overlay()
imgOverlay2 = database.get_imgTarget_overlay2()

cube = database.get_cube()

hT, wT, cT = imgTarget.shape
hT2, wT2, cT2 = imgTarget2.shape

orb = cv2.ORB_create(nfeatures=1000)
kp1, des1 = orb.detectAndCompute(imgTarget, None)
kp2, des2 = orb.detectAndCompute(imgTarget2, None)

if debug:
    imgPoints = cv2.drawKeypoints(imgTarget, kp1, None)              #//debug poster keypoints
    imgPoints2 = cv2.drawKeypoints(imgTarget2, kp2, None)

def draw(img, corners, imgpts):
    imgpts = np.int32(imgpts).reshape(-1,2)
    # draw ground floor in green
    img = cv2.drawContours(img, [imgpts[:4]],-1,(0,255,0),-3)
    # draw pillars in blue color
    for i,j in zip(range(4),range(4,8)):
        img = cv2.line(img, tuple(imgpts[i]), tuple(imgpts[j]),(255),3)
    # draw top layer in red color
    img = cv2.drawContours(img, [imgpts[4:]],-1,(0,0,255),3)
    return img


# stackImages
def stackImages(imgArray,scale,lables=[]):
    sizeW= imgArray[0][0].shape[1]
    sizeH = imgArray[0][0].shape[0]
    rows = len(imgArray)
    cols = len(imgArray[0])
    rowsAvailable = isinstance(imgArray[0], list)
    if rowsAvailable:
        for x in range(0, rows):
            for y in range(0, cols):
                imgArray[x][y] = cv2.resize(imgArray[x][y], (sizeW,sizeH), None, scale, scale)
                if len(imgArray[x][y].shape) == 2 : imgArray[x][y] = cv2.cvtColor(imgArray[x][y], cv2.COLOR_GRAY2BGR)
        imageBlank = np.zeros((sizeH, sizeW, 3), np.uint8)
        hor = [imageBlank]*rows
        hor_con = [imageBlank]*rows
        for x in range(0, rows):
            hor[x] = np.hstack(imgArray[x])
            hor_con[x] = np.concatenate(imgArray[x])
        ver = np.vstack(hor)
        ver_con = np.concatenate(hor)
    else:
        for x in range(0, rows):
            imgArray[x] = cv2.resize(imgArray[x], (sizeW, sizeH), None, scale, scale)
            if len(imgArray[x].shape) == 2: imgArray[x] = cv2.cvtColor(imgArray[x], cv2.COLOR_GRAY2BGR)
        hor = np.hstack(imgArray)
        hor_con = np.concatenate(imgArray)
        ver = hor
    if len(lables) != 0:
        eachImgWidth = int(ver.shape[1] / cols)
        eachImgHeight = int(ver.shape[0] / rows)

        print(eachImgHeight)

        for d in range(0, rows):
            for c in range (0, cols):
                cv2.rectangle(ver, (c*eachImgWidth, eachImgHeight*d), (c*eachImgWidth+len(lables[d]) * 13 + 27, 30 + eachImgHeight * d), (255, 255, 255), cv2.FILLED)
                cv2.putText(ver, lables[d], (eachImgWidth*c+10, eachImgHeight*d+20), cv2.FONT_HERSHEY_COMPLEX, 0.7, (255, 0, 255), 2)
    return ver

while True:
    imgWebcam = webcam.get_current_frame()
    imgAug = imgWebcam.copy()
    imgAug2 = imgWebcam.copy()
    kpW, desW = orb.detectAndCompute(imgWebcam, None)

    bf = cv2.BFMatcher()

    matches = bf.knnMatch(des1, desW, k=2)
    matches2 = bf.knnMatch(des2, desW, k=2)

    good = []
    good2 = []

    for m, n in matches:
        if m.distance < 0.75 * n.distance:
            good.append(m)

    for m, n in matches2:
        if m.distance < 0.75 * n.distance:
            good2.append(m)

    if debug:
        print("Poster1 matches: " + str(len(good)))
        print("Poster2 matches: " + str(len(good2)))                                                                                                    #//number of good matches
        imgFeatures = cv2.drawMatches(imgTarget, kp1, imgWebcam, kpW, good, None, flags=2)
        imgFeatures2 = cv2.drawMatches(imgTarget2, kp2, imgWebcam, kpW, good2, None, flags=2)

    if len(good) > 20 or len(good2) > 20:

        if len(good) > 20:
            srcPts = np.float32([kp1[m.queryIdx].pt for m in good]).reshape(-1, 1, 2)
            dstPts = np.float32([kpW[m.trainIdx].pt for m in good]).reshape(-1, 1, 2)
            matrix, mask = cv2.findHomography(srcPts, dstPts, cv2.RANSAC, 5)
            pts = np.float32([[0, 0], [0,hT], [wT, hT], [wT, 0]]).reshape(-1, 1, 2)
            dst = cv2.perspectiveTransform(pts, matrix)
            if debug:
                print("Matrix poster 1")
                print(matrix)


        if len(good2) > 20:
            srcPts2 = np.float32([kp2[m.queryIdx].pt for m in good2]).reshape(-1, 1, 2)
            dstPts2 = np.float32([kpW[m.trainIdx].pt for m in good2]).reshape(-1, 1, 2)
            matrix2, mask2 = cv2.findHomography(srcPts2, dstPts2, cv2.RANSAC, 5)
            pts2 = np.float32([[0, 0], [0,hT2], [wT2, hT2], [wT2, 0]]).reshape(-1, 1, 2)
            dst2 = cv2.perspectiveTransform(pts2, matrix2)
            if debug:
                print("Matrix poster 2")
                print(matrix2)                                                                                                         #//matrix for debug

        if len(good) > 20:
            imgWarp = cv2.warpPerspective(imgOverlay, matrix, (imgWebcam.shape[1], imgWebcam.shape[0]))
            imgAug = cv2.bitwise_or(imgWarp, imgAug)
            imgWebcam = cv2.bitwise_or(imgWarp, imgWebcam)
            if debug:
                img = cv2.polylines(imgAug, [np.int32(dst)], True, (255, 0, 255), 3)

        if len(good2) > 20:
            imgWarp2 = cv2.warpPerspective(imgOverlay2, matrix2, (imgWebcam.shape[1], imgWebcam.shape[0]))
            imgAug2 = cv2.bitwise_or(imgWarp2, imgAug2)
            imgWebcam = cv2.bitwise_or(imgWarp2, imgWebcam)

            if debug:
                img2 = cv2.polylines(imgAug2, [np.int32(dst2)], True, (255, 0, 255), 3)

        if debug and len(good) > 20 and len(good2) > 20:
            imgStacked = stackImages(([imgFeatures, imgPoints], [imgFeatures2, imgPoints2]), 0.5)
            imgStacked2 = stackImages(([imgWarp, img], [imgWarp2, img2]), 0.5)
            cv2.imshow('stack', imgStacked)                                                                 #//DEBUG MODE STACKED
            cv2.imshow('stack2', imgStacked2)

    cv2.imshow('Poster', imgTarget)
    cv2.imshow('Poster2', imgTarget2)
    cv2.imshow('Webcam', imgWebcam)
    cv2.waitKey(0)