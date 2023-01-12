import { readFileSync } from "fs";
import { parseString } from "xml2js"
import { svgPathProperties } from "svg-path-properties";
import smallestEnclosingCircle from "./smallestEnclosingCircle.js";

// We don't want to go all the way to the edge, it's messy there.
const timestep = 1;
const sizeReduction = 0.95;

// Load SVG
const filename = process.argv[2];
const svgText = readFileSync(filename);
parseString(svgText, (err, result) => {
    // TODO: Handle multiple paths
    var path = result.svg.path[0].$.d;

    // Convert SVG path to line segments
    const properties = new svgPathProperties(path);
    const points = [];
    const len = properties.getTotalLength();
    for (let t = 0; t < len; t += timestep) {
        var point = properties.getPointAtLength(t);
        points.push(point);
    }

    // SVG is Y down (like a monitor), convert to Y up
    for (const point of points) {
        point.y = 0 - point.y;
    }

    // Normalize points inside a unit circle
    const circle  = smallestEnclosingCircle(points);
    for (const point of points) {
        point.x = (point.x - circle.x) / circle.r * sizeReduction;
        point.y = (point.y - circle.y) / circle.r * sizeReduction;
    }

    // Find the closest point to the edge, and reorder the path to start with that point
    // TODO: This assumes a circular path.
    let biggestRadius = 0;
    let closestPointIndex = 0;
    for (let i = 0; i < points.length; i++) {
        const point = points[i];
        const radius = Math.sqrt(point.x * point.x + point.y * point.y);
        if (radius >= biggestRadius) {
            biggestRadius = radius;
            closestPointIndex = i;
        }
    }
    points.push(...points.splice(0, closestPointIndex));
    points.push(points[0]); // Make the path circular

    // Walk in a straight line from and back out to the edge
    const edge = { x: points[0].x, y: points[0].y };
    if (Math.abs(edge.x) > Math.abs(edge.y)) {
        edge.x = Math.sign(edge.x) * Math.sqrt(1 - edge.y * edgey.y);
    } else {
        edge.y = Math.sign(edge.y) * Math.sqrt(1 - edge.x * edge.x);
    }
    points.splice(0, 0, edge);
    points.push(edge);

    // Emit as THR
    for (const point of points) {
        var theta = Math.atan2(point.x, point.y); // Intentional reversal of x and y here to get THR polar coordinates
        var rho = Math.sqrt(point.x * point.x + point.y * point.y);
        console.log(`${theta} ${rho}`);
    }
});


