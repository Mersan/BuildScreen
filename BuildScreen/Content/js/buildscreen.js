/*global $, _ */


( function () {

    'use strict';

    function scrollUp () {
        $( '.iframe-content' ).animate( { top: '0' }, 10000 );
    }

    function scrollDown () {
        $('.iframe-content').animate({ top: '-900px' }, 10000);
    }

    setTimeout( function () {
        scrollDown();
    }, 3000 );

    setTimeout( function () {
        scrollUp();
    }, 14000 );


}() );