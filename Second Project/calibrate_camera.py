import numpy as np
import cv2
import glob

IMAGE_PATH = 'chessboard_distortion.png'

# termination criteria - stop the algorithm if the accuracy is reached, or after 30 iterations
criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 30, 0.001)

# chessboard is a 8x8 grid, without outside grid is a 7x7
# object points
objp = np.zeros((7*7, 3), np.float32)
objp[:,:2] = np.mgrid[0:7, 0:7].T.reshape(-1, 2)

# creating arrays for image and objet points
objpoints = [] # 3dpoint in real world space
imgpoints = [] # 2dpoints in image plane

# find png files in the same path as the code
images = glob.glob('./chessboard_distortion.png')
print(images)
for fname in images:
    img = cv2.imread(fname)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY) # grayscale

    # Find the chessboard corners
    ret, corners = cv2.findChessboardCorners(gray, (7,7))
    print(ret)
    print(corners)
    # If found corners, add object points and image points
    if ret == True:
        objpoints.append(objp)

        # refine corner locations
        corners2 = cv2.cornerSubPix(gray, corners, (11,11), (-1,-1), criteria)
        imgpoints.append(corners2)

        # Draw and display the corners
        img = cv2.drawChessboardCorners(img, (7,7), corners2, ret)
        cv2.imshow('Chessboard Corners', img)

        # gray.shape[::-1] -> vector of integers, containing as many elements as the number of views of the calibration pattern
        # Each element is he number of points in each view. The elements should all be the same and equal to the number of feature points on the calibration pattern
        ret, mtx, dist, rvecs, tvecs = cv2.calibrateCamera(objpoints, imgpoints, gray.shape[::-1], None, None, criteria)
        cv2.waitKey(500)

img = cv2.imread(IMAGE_PATH)
h, w = img.shape[:2]
newcameramtx, roi = cv2.getOptimalNewCameraMatrix(mtx, dist, (w,h), 0, (w,h))
# revert distortion
dst = cv2.undistort(img, mtx, dist, None, newcameramtx)

# crop
x,y,w,h = roi
dst = dst[y:y+h, x:x+w]
cv2.imwrite("calibration_result.png", dst) # save image

# cv2.destroyAllWindows()

cv2.waitKey()