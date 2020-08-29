
$(function () {

        $("#ListRegions").change(() => {

            $.ajax({
                url: "/Enumerations/Villes_Region",
                method: "GET",
                data: { UID_R: '' + $("#ListRegions").val() + '' },
                success: function (response) {
                    $("#UID_V").children().remove();
                    $("#UID_V").append('<option name="listVilles" value="-1">________________________________________</option>')
                    response = JSON.parse(response);
                    $.each(response, (index, ville) => {
                        $("#UID_V").append('<option name="listVilles" value="' + ville.UID + '">' + ville.Nom + '</option>');
                    })
                },
                error: function (err) {
                    console.log(err);
                }
            });
          
        })


});

