// https://github.com/rowanwins/smallest-enclosing-circle/blob/master/src/main.js
//  Modified to remove recursion

export default function wetzls (points) {
    // clone and then shuffle the points
    const clonedPoints = points.slice()
    shuffle(clonedPoints)
    return solve(clonedPoints, points.length)
}

function shuffle(a) {
    let j, x, i
    for (i = a.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1))
        x = a[i]
        a[i] = a[j]
        a[j] = x
    }
    return a
}

function solve(points, n) {
     let result;
     let next = mec(points, n, [], 0, circle => { result = circle; });
     while (next) {
         next = next();
     }
     return result;
}

function mec(points, n, boundary, b, callback) {
    if (b === 3) return callback(calcCircle3(boundary[0], boundary[1], boundary[2]));
    else if (n === 1 && b === 0) return callback({x: points[0].x, y: points[0].y, r: 0});
    else if (n === 0 && b === 2) return callback(calcCircle2(boundary[0], boundary[1]));
    else if (n === 1 && b === 1) return callback(calcCircle2(boundary[0], points[0]));
    else {
        return () => mec(points, n - 1, boundary, b, localCircle => {
            if (!isInCircle(points[n - 1], localCircle)) {
                boundary[b++] = points[n - 1]
                return mec(points, n - 1, boundary, b, callback);
            } else {
                return () => callback(localCircle);
            }
        });
    }

    return localCircle;
}

function calcCircle3(p1, p2, p3) {
    const p1x = p1.x,
        p1y = p1.y,
        p2x = p2.x,
        p2y = p2.y,
        p3x = p3.x,
        p3y = p3.y,

        a = p2x - p1x,
        b = p2y - p1y,
        c = p3x - p1x,
        d = p3y - p1y,
        e = a * (p2x + p1x) * 0.5 + b * (p2y + p1y) * 0.5,
        f = c * (p3x + p1x) * 0.5 + d * (p3y + p1y) * 0.5,
        det = a * d - b * c,

        cx = (d * e - b * f) / det,
        cy = (-c * e + a * f) / det

    return {x: cx, y: cy, r: Math.sqrt((p1x - cx) * (p1x - cx) + (p1y - cy) * (p1y - cy))}
}

function calcCircle2(p1, p2) {
    const p1x = p1.x,
        p1y = p1.y,
        p2x = p2.x,
        p2y = p2.y,
        cx = 0.5 * (p1x + p2x),
        cy = 0.5 * (p1y + p2y)

    return {x: cx, y: cy, r: Math.sqrt((p1x - cx) * (p1x - cx) + (p1y - cy) * (p1y - cy))}
}

function isInCircle (p, c) {
    return ((c.x - p.x) * (c.x - p.x) + (c.y - p.y) * (c.y - p.y) <= c.r * c.r)
}