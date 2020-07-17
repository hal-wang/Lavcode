using Windows.UI.Xaml;

/**
 *                        d*##$.
 *  zP"""""$e.           $"    $o
 * 4$       '$          $"      $
 * '$        '$        J$       $F
 *  'b        $k       $>       $
 *   $k        $r     J$       d$
 *   '$         $     $"       $~
 *    '$        "$   '$E       $
 *     $         $L   $"      $F ...
 *      $.       4B   $      $$$*"""*b
 *      '$        $.  $$     $$      $F
 *       "$       R$  $F     $"      $
 *        $k      ?$ u*     dF      .$
 *        ^$.      $$"     z$      u$$$$e
 *         #$b             $E.dW@e$"    ?$
 *          #$           .o$$# d$$$$c    ?F
 *           $      .d$$#" . zo$>   #$r .uF
 *           $L .u$*"      $&$$$k   .$$d$$F
 *            $$"            ""^"$$$P"$P9$
 *           JP              .o$$$$u:$P $$
 *           $          ..ue$"      ""  $"
 *          d$          $F              $
 *          $$     ....udE             4B
 *           #$    """"` $r            @$
 *            ^$L        '$            $F
 *              RN        4N           $
 *               *$b                  d$
 *                $$k                 $F
 *                 $$b                $F
 *                  $""               $F
 *                  '$                $
 *                   $L               $
 *                   '$               $
 *                    $               $
 */

namespace Hubery.Lavcode.Uwp
{
    /// <summary>
    /// 用于向VM传递View，建议少用，不符合MVVM设计思想
    /// 主要用来配合FrameworkElement.FindName找元素，用于贴靠提醒ShowSticky
    /// </summary>
    public interface IElementViewModel
    {
        FrameworkElement View { get; set; }
    }
}
