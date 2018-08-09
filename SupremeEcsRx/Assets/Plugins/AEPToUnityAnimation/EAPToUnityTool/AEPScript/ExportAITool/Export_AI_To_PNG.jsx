#target Illustrator

//  script.name = exportLayersAsCSS_PNGs.jsx;
//  script.description = mimics the Save for Web, export images as CSS Layers (images only);
//  script.requirements = an open document; tested with CS5 on Windows. 
//  script.parent = carlos canto // 05/24/13; All rights reseved
//  script.elegant = false;


/**
* export layers as PNG
* @author Niels Bosma
*/
// Adapted to export images as CSS Layers by CarlosCanto


if (app.documents.length>0) {
    main();
}
else alert('Cancelled by user');


function main() {
    var document = app.activeDocument;
    var afile = document.fullName;
    var filename = afile.name.split('.')[0];


    var folder = afile.parent.selectDlg("Export as CSS Layers (images only)...");


    if(folder != null)
    { 
        var activeABidx = document.artboards.getActiveArtboardIndex();
        var activeAB = document.artboards[activeABidx]; // get active AB        
        var abBounds = activeAB.artboardRect;// left, top, right, bottom


        showAllLayers();
        var docBounds = document.visibleBounds;
        activeAB.artboardRect = docBounds;


        var options = new ExportOptionsPNG24();
        options.antiAliasing = true;
        options.transparency = true;
        options.artBoardClipping = true;

        var n = document.layers.length;
        hideAllLayers ();
        for(var i=n-1, k=0; i>=0; i--, k++)
        {
            //hideAllLayers();
            var layer = document.layers[i];
            layer.visible = true;
            

            var file = new File(folder.fsName + '/' +layer.name+".png");

            document.exportFile(file,ExportType.PNG24,options);
            layer.visible = false;
        }

        showAllLayers();
        activeAB.artboardRect = abBounds;
    }


    function hideAllLayers()
    {
        forEach(document.layers, function(layer) {
            layer.visible = false;
        });
    }


    function showAllLayers()
    {
        forEach(document.layers, function(layer) {
            layer.visible = true;
        }); 
    }


    function forEach(collection, fn)
    {
        var n = collection.length;
        for(var i=0; i<n; ++i)
        {
            fn(collection[i]);
        }
    }
}