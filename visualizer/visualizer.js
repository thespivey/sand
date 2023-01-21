window.addEventListener("DOMContentLoaded", async () => {

    const response = await fetch("http://localhost:8080/Logo/Kraken/kraken.thr");
    const thr = await response.text();
    
    const path = thr.split("\n").map(line => {
        if (!line.startsWith("#")) {
            const [ theta, rho ] = line.split(" ").filter(_ => _).map(n => parseFloat(n));

            // Jumble sin and cos for theta/rho format, invert Y for canvas coordinates
            return {
                x: Math.sin(theta) * rho,
                y: -Math.cos(theta) * rho
            }
        }
    }).filter(_ => _);

    // TODO: interpolate points (on an arc!)

    const canvas = document.querySelector("canvas");
    draw(canvas, path);
    window.addEventListener("resize", () => draw(canvas, path));
});

function draw(canvas, path) {
    canvas.width = canvas.clientWidth;
    canvas.height = canvas.clientHeight;
    const tableSize = Math.min(canvas.clientWidth, canvas.clientHeight);
    const tableRadius = tableSize / 2;
    const ballRadius = 12/1000 * tableSize;
    const sandEdgeThicknes = 2/1000 * tableSize;
    
    /** @type { CanvasRenderingContext2D }*/
    const ctx = canvas.getContext("2d");
    ctx.clearRect(0, 0, tableSize, tableSize);

    ctx.arc(tableRadius, tableRadius, tableRadius, 0, Math.PI*2);
    ctx.stroke();

    let lastPoint;
    for (const point of path) {
        let range = { start: 0, end: 2*Math.PI}
        if (lastPoint) {
            const direction = Math.atan2(point.y - lastPoint.y, point.x - lastPoint.x);
            range = { start: direction - Math.PI / 2, end: direction + Math.PI / 2}
        }
        lastPoint = point;

        ctx.beginPath();
        ctx.arc((point.x + 1) * tableRadius, (point.y + 1) * tableRadius, ballRadius + sandEdgeThicknes, range.start, range.end);
        ctx.fillStyle = "#000000";
        ctx.fill();

        ctx.beginPath();
        ctx.arc((point.x + 1) * tableRadius, (point.y + 1) * tableRadius, ballRadius, 0, Math.PI * 2);
        ctx.fillStyle = "#CCCCCC";
        ctx.fill();
    }
}